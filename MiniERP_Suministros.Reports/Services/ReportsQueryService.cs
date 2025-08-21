// RUTA: MiniERP_Suministros.Reports/Services/ReportsQueryService.cs
// Descripción: Servicio ADO.NET para consultar datos de los reportes desde la misma base de datos del servidor.
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MiniERP_Suministros.Reports.Services
{
    public class ReportsQueryService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; // usa misma DB

        public async Task<DataTable> GetVentasPeriodoCategoriaAsync(DateTime fechaInicio, DateTime fechaFin, int? categoriaId, int? customerId)
        {
            const string sql = @"
SELECT
    pc.Name AS CategoryName,
    o.Id AS OrderId,
    o.CreatedDate AS OrderDate,
    c.Name AS CustomerName,
    p.Name AS ProductName,
    od.Quantity,
    od.UnitPrice,
    (od.UnitPrice * od.Quantity - od.Discount) AS DetailTotal,
    o.Discount AS OrderDiscount
FROM [dbo].[AppOrders] o
JOIN [dbo].[AppOrderDetails] od ON od.OrderId = o.Id
JOIN [dbo].[AppProducts] p ON p.Id = od.ProductId
JOIN [dbo].[AppProductCategories] pc ON pc.Id = p.ProductCategoryId
JOIN [dbo].[AppCustomers] c ON c.Id = o.CustomerId
WHERE o.CreatedDate >= @fi AND o.CreatedDate < @ff
  AND (@catId IS NULL OR p.ProductCategoryId = @catId)
  AND (@custId IS NULL OR o.CustomerId = @custId)
ORDER BY pc.Name, o.Id, p.Name;";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@fi", fechaInicio);
                cmd.Parameters.AddWithValue("@ff", fechaFin);
                cmd.Parameters.AddWithValue("@catId", (object)categoriaId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@custId", (object)customerId ?? DBNull.Value);

                var table = new DataTable("Items");
                await cn.OpenAsync();
                da.Fill(table);
                table.Columns["DetailTotal"].ReadOnly = false; // CR suele necesitar columnas editables para fórmulas
                return table;
            }
        }

        public async Task<DataTable> GetResumenClientesAsync(DateTime fechaInicio, DateTime fechaFin, int? customerId, decimal? montoMinimo)
        {
            const string sql = @"
WITH OrderTotals AS (
    SELECT o.Id, o.CustomerId, o.CreatedDate,
           SUM(od.UnitPrice * od.Quantity - od.Discount) AS TotalDetalle,
           o.Discount AS OrderDiscount
    FROM [dbo].[AppOrders] o
    JOIN [dbo].[AppOrderDetails] od ON od.OrderId = o.Id
    WHERE o.CreatedDate >= @fi AND o.CreatedDate < @ff
    GROUP BY o.Id, o.CustomerId, o.CreatedDate, o.Discount
)
SELECT
    c.Id AS CustomerId,
    c.Name AS CustomerName,
    COUNT(ot.Id) AS OrdersCount,
    SUM(ot.TotalDetalle - ISNULL(ot.OrderDiscount, 0)) AS OrdersTotal,
    CASE WHEN COUNT(ot.Id) = 0 THEN 0 ELSE SUM(ot.TotalDetalle - ISNULL(ot.OrderDiscount, 0)) * 1.0 / NULLIF(COUNT(ot.Id), 0) END AS AvgPerOrder,
    MAX(ot.CreatedDate) AS LastOrderDate
FROM [dbo].[AppCustomers] c
LEFT JOIN OrderTotals ot ON ot.CustomerId = c.Id
WHERE (@custId IS NULL OR c.Id = @custId)
GROUP BY c.Id, c.Name
HAVING (@minMonto IS NULL OR SUM(ot.TotalDetalle - ISNULL(ot.OrderDiscount, 0)) >= @minMonto)
ORDER BY OrdersTotal DESC;";

            using (var cn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@fi", fechaInicio);
                cmd.Parameters.AddWithValue("@ff", fechaFin);
                cmd.Parameters.AddWithValue("@custId", (object)customerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@minMonto", (object)montoMinimo ?? DBNull.Value);

                var table = new DataTable("Clientes");
                await cn.OpenAsync();
                da.Fill(table);
                return table;
            }
        }
    }
}
