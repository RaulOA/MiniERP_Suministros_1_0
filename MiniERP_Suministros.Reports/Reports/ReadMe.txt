RUTA: MiniERP_Suministros.Reports/Reports/ReadMe.txt
Descripci�n: Carpeta contenedora para reportes Crystal Reports (.rpt). Coloque aqu� los archivos .rpt.

- VentasPorPeriodoYCategoria.rpt: usa dataset "VentasPeriodoCategoria" con tablas:
  * Items: CategoryName (string), OrderId (int), OrderDate (DateTime), CustomerName (string), ProductName (string), Quantity (int), UnitPrice (decimal), DetailTotal (decimal), OrderDiscount (decimal)

- ResumenComprasPorCliente.rpt: usa dataset "ResumenClientes" con tablas:
  * Clientes: CustomerId (int), CustomerName (string), OrdersCount (int), OrdersTotal (decimal), AvgPerOrder (decimal), LastOrderDate (DateTime)

Sugerencias de dise�o:
- Configure par�metros en el .rpt: FechaInicio, FechaFin, CategoriaId (Number, opcional), CustomerId (Number, opcional), MontoMinimo (Currency, opcional).
- Agrupe por categor�a y por pedido en VentasPorPeriodoYCategoria.
- En ResumenComprasPorCliente, ordene por OrdersTotal desc y agregue gr�fico opcional.
