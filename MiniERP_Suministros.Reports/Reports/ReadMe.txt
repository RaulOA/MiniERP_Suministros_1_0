RUTA: MiniERP_Suministros.Reports/Reports/ReadMe.txt
Descripción: Carpeta contenedora para reportes Crystal Reports (.rpt). Coloque aquí los archivos .rpt.

- VentasPorPeriodoYCategoria.rpt: usa dataset "VentasPeriodoCategoria" con tablas:
  * Items: CategoryName (string), OrderId (int), OrderDate (DateTime), CustomerName (string), ProductName (string), Quantity (int), UnitPrice (decimal), DetailTotal (decimal), OrderDiscount (decimal)

- ResumenComprasPorCliente.rpt: usa dataset "ResumenClientes" con tablas:
  * Clientes: CustomerId (int), CustomerName (string), OrdersCount (int), OrdersTotal (decimal), AvgPerOrder (decimal), LastOrderDate (DateTime)

Sugerencias de diseño:
- Configure parámetros en el .rpt: FechaInicio, FechaFin, CategoriaId (Number, opcional), CustomerId (Number, opcional), MontoMinimo (Currency, opcional).
- Agrupe por categoría y por pedido en VentasPorPeriodoYCategoria.
- En ResumenComprasPorCliente, ordene por OrdersTotal desc y agregue gráfico opcional.
