// RUTA: MiniERP_Suministros.Reports/Controllers/ReportsController.cs
// Descripci�n: Controlador MVC para filtros y exportaci�n de reportes a PDF.
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using MiniERP_Suministros.Reports.Services;

namespace MiniERP_Suministros.Reports.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ReportsQueryService _queryService = new ReportsQueryService();

        // GET: Reports/VentasPeriodoCategoria
        public ActionResult VentasPeriodoCategoria() => View();

        // POST: Reports/VentasPeriodoCategoriaExport
        [HttpPost]
        public async Task<ActionResult> VentasPeriodoCategoriaExport(DateTime fechaInicio, DateTime fechaFin, int? categoriaId, int? customerId)
        {
            if (fechaFin <= fechaInicio) return new HttpStatusCodeResult(400, "Rango de fechas inv�lido");

            var table = await _queryService.GetVentasPeriodoCategoriaAsync(fechaInicio, fechaFin, categoriaId, customerId);

#if USE_CRYSTAL
            // Requiere instalar Crystal Reports runtimes y referencias.
            using (var report = new CrystalDecisions.CrystalReports.Engine.ReportDocument())
            {
                var rptPath = Server.MapPath("~/Reports/VentasPorPeriodoYCategoria.rpt");
                report.Load(rptPath);
                var ds = new System.Data.DataSet("VentasPeriodoCategoria");
                ds.Tables.Add(table);
                report.SetDataSource(ds);
                report.SetParameterValue("FechaInicio", fechaInicio);
                report.SetParameterValue("FechaFin", fechaFin);
                report.SetParameterValue("CategoriaId", (object)categoriaId ?? DBNull.Value);
                report.SetParameterValue("CustomerId", (object)customerId ?? DBNull.Value);
                using (var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(ReadFully(stream), "application/pdf", $"Ventas_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.pdf");
                }
            }
#else
            // Fallback: exportaci�n CSV sencilla para validar flujo sin Crystal.
            var csv = CsvFromDataTable(table);
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"Ventas_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.csv");
#endif
        }

        // GET: Reports/ResumenClientes
        public ActionResult ResumenClientes() => View();

        // POST: Reports/ResumenClientesExport
        [HttpPost]
        public async Task<ActionResult> ResumenClientesExport(DateTime fechaInicio, DateTime fechaFin, int? customerId, decimal? montoMinimo)
        {
            if (fechaFin <= fechaInicio) return new HttpStatusCodeResult(400, "Rango de fechas inv�lido");

            var table = await _queryService.GetResumenClientesAsync(fechaInicio, fechaFin, customerId, montoMinimo);

#if USE_CRYSTAL
            using (var report = new CrystalDecisions.CrystalReports.Engine.ReportDocument())
            {
                var rptPath = Server.MapPath("~/Reports/ResumenComprasPorCliente.rpt");
                report.Load(rptPath);
                var ds = new System.Data.DataSet("ResumenClientes");
                ds.Tables.Add(table);
                report.SetDataSource(ds);
                report.SetParameterValue("FechaInicio", fechaInicio);
                report.SetParameterValue("FechaFin", fechaFin);
                report.SetParameterValue("CustomerId", (object)customerId ?? DBNull.Value);
                report.SetParameterValue("MontoMinimo", (object)montoMinimo ?? DBNull.Value);
                using (var stream = report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(ReadFully(stream), "application/pdf", $"ResumenClientes_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.pdf");
                }
            }
#else
            var csv = CsvFromDataTable(table);
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"ResumenClientes_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}.csv");
#endif
        }

        private static string CsvFromDataTable(DataTable dt)
        {
            using (var sw = new StringWriter())
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sw.Write(",");
                    sw.Write(dt.Columns[i].ColumnName);
                }
                sw.WriteLine();
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i > 0) sw.Write(",");
                        var val = row[i] == DBNull.Value ? string.Empty : row[i].ToString();
                        // Escapar comas y comillas
                        if (val != null && (val.Contains(",") || val.Contains("\"")))
                        {
                            val = "\"" + val.Replace("\"", "\"\"") + "\"";
                        }
                        sw.Write(val);
                    }
                    sw.WriteLine();
                }
                return sw.ToString();
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream()) { input.CopyTo(ms); return ms.ToArray(); }
        }
    }
}
