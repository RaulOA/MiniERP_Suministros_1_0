RUTA: MiniERP_Suministros.Reports/Docs/Guia-Reportes-Crystal-PDF.md
Descripci�n: Gu�a paso a paso para un usuario sin experiencia previa, para visualizar y exportar a PDF los reportes en el proyecto MiniERP_Suministros.Reports.

# Gu�a paso a paso para ver reportes en PDF (Crystal Reports)

Esta gu�a te lleva desde cero hasta descargar los PDF de los dos reportes preparados:
- Ventas por Per�odo y Categor�a
- Resumen de Compras por Cliente

Los formularios ya est�n creados en el sitio y se conectan a la misma base de datos de la soluci�n.

---

## 1) Verificar base de datos lista (una sola vez)
1. Establece como proyecto de inicio: MiniERP_Suministros.Server.
2. Ejecuta el proyecto (F5) una vez. Esto aplica migraciones y, si la BD est� vac�a, siembra datos demo.
   - Nombre BD: MiniERP_Suministros en (localdb)\\MSSQLLocalDB.
3. Si ya tienes la BD con datos, no hace nada adicional.

---

## 2) (Opcional) Probar el flujo sin Crystal
Ya validaste que se generan TXT/CSV correctamente. Si quieres repetir:
1. Establece como proyecto de inicio: MiniERP_Suministros.Reports.
2. Ejecuta (F5) y entra a:
   - Reportes > Ventas por Per�odo y Categor�a
   - Reportes > Resumen de Compras por Cliente
3. Elige un rango que cubra los �ltimos 6 meses y Exportar (descargar� CSV de prueba).

---

## 3) Instalar Crystal Reports para exportar PDF
Crystal no est� en NuGet; inst�lalo en tu equipo de desarrollo:
1. Descarga e instala "SAP Crystal Reports, developer version for Microsoft Visual Studio" (Add-in y Runtime) desde el sitio de SAP.
2. Reinicia Visual Studio al finalizar.
3. Arquitectura recomendada: x64 (usa runtime x64 y compila el proyecto en x64).

---

## 4) Agregar referencias Crystal al proyecto MVC (si no aparecen solas)
1. En MiniERP_Suministros.Reports: Referencias > Agregar Referencia�
2. Agrega:
   - CrystalDecisions.CrystalReports.Engine
   - CrystalDecisions.Shared
   - CrystalDecisions.ReportSource

---

## 5) Habilitar exportaci�n a PDF
1. Propiedades del proyecto (MiniERP_Suministros.Reports) > Compilar.
2. En "S�mbolos de compilaci�n condicional" agrega: USE_CRYSTAL
3. Guarda y recompila.

Con esto, las acciones de exportaci�n generar�n PDF en lugar de CSV.

---

## 6) Colocar los archivos .rpt en la carpeta Reports
Ruta esperada:
- C:\\Users\\User\\source\\repos\\MiniERP_Suministros_1_0\\MiniERP_Suministros.Reports\\Reports\\

Archivos y definici�n de datasets/tablas:
1) VentasPorPeriodoYCategoria.rpt
   - Dataset: VentasPeriodoCategoria
   - Tabla: Items
   - Campos (Items): CategoryName, OrderId, OrderDate, CustomerName, ProductName, Quantity, UnitPrice, DetailTotal, OrderDiscount
   - Par�metros: FechaInicio, FechaFin, CategoriaId (opcional), CustomerId (opcional)
   - Dise�o sugerido: Grupo por CategoryName y por OrderId; detalle con ProductName, Quantity, UnitPrice, DetailTotal; totales por pedido y categor�a; total general.

2) ResumenComprasPorCliente.rpt
   - Dataset: ResumenClientes
   - Tabla: Clientes
   - Campos (Clientes): CustomerId, CustomerName, OrdersCount, OrdersTotal, AvgPerOrder, LastOrderDate
   - Par�metros: FechaInicio, FechaFin, CustomerId (opcional), MontoMinimo (opcional)
   - Dise�o sugerido: Grupo por CustomerName; ordenar por OrdersTotal desc; gr�fico de barras opcional.

Nota: Puedes dise�ar enlazando un .xsd con estas tablas o confiar en el DataSet que pasa la app en tiempo de ejecuci�n.

---

## 7) Ejecutar y descargar PDF
1. Establece MiniERP_Suministros.Reports como proyecto de inicio.
2. Ejecuta (F5) y navega a:
   - /Reports/VentasPeriodoCategoria
   - /Reports/ResumenClientes
3. Selecciona un rango de fechas (�ltimos 6 meses) y Exportar. Debes obtener un PDF.

---

## 8) Conexi�n a base de datos (ya configurada)
- Web.config de MiniERP_Suministros.Reports usa:
  - DefaultConnection = Server=(localdb)\\MSSQLLocalDB;Database=MiniERP_Suministros;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true
- No necesitas configurar conexi�n dentro del .rpt: la app inyecta DataSet.

---

## 9) Rutas �tiles en el proyecto
- Formularios de filtros:
  - Views/Reports/VentasPeriodoCategoria.cshtml
  - Views/Reports/ResumenClientes.cshtml
- Controlador:
  - Controllers/ReportsController.cs (acciones ...Export usan Crystal si USE_CRYSTAL est� definido)
- Consultas SQL (ADO.NET):
  - Services/ReportsQueryService.cs
- Carpeta de reportes:
  - Reports/ (colocar aqu� los .rpt)

---

## 10) Problemas comunes
- Falta de CrystalDecisions.*: instala el add-in/runtime y agrega referencias. Limpia y recompila.
- Conflicto x64/x86: usa runtime Crystal x64 y compila el proyecto en x64.
- PDF vac�o: revisa el rango de fechas y que existan datos.
- Conexi�n: si usas otra instancia, ajusta Web.config > connectionStrings > DefaultConnection.

---

Listo. Con estos pasos podr�s colocar los .rpt y descargar los reportes finales en PDF desde el navegador.