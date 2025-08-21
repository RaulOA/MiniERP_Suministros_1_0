RUTA: MiniERP_Suministros.Reports/README-Reports-Setup.md
Descripci�n: Gu�a r�pida para habilitar Crystal Reports en este proyecto.

1) Instalar Crystal Reports Runtime y referencias NuGet (solo en desarrollo/servidor Windows):
   - Instalar SAP Crystal Reports, developer version for Microsoft Visual Studio (runtime + addin).
   - Agregar referencias a: CrystalDecisions.CrystalReports.Engine, CrystalDecisions.Shared, CrystalDecisions.ReportSource.

2) Colocar los archivos .rpt en: /Reports
   - VentasPorPeriodoYCategoria.rpt (dataset: VentasPeriodoCategoria, tabla: Items)
   - ResumenComprasPorCliente.rpt (dataset: ResumenClientes, tabla: Clientes)
   - Configurar par�metros en el .rpt: FechaInicio, FechaFin, CategoriaId, CustomerId, MontoMinimo.

3) Compilar con s�mbolo de compilaci�n USE_CRYSTAL para exportar a PDF:
   - Propiedades del proyecto -> Compilaci�n -> S�mbolos de compilaci�n condicional: USE_CRYSTAL
   - O en Web.config (s�lo si usa transformaciones) configure el s�mbolo en la configuraci�n apropiada.

4) Seguridad/Conexi�n:
   - Este proyecto usa la misma cadena de conexi�n que el servidor (name=DefaultConnection) definida en Web.config.

5) Endpoints:
   - GET /Reports/VentasPeriodoCategoria (formulario de filtros)
   - POST /Reports/VentasPeriodoCategoriaExport (descarga PDF/CSV)
   - GET /Reports/ResumenClientes (formulario de filtros)
   - POST /Reports/ResumenClientesExport (descarga PDF/CSV)
