RUTA: MiniERP_Suministros.Reports/Docs/Guia-Reportes-Crystal-PDF.md
Descripción: Guía paso a paso para un usuario sin experiencia previa, para visualizar y exportar a PDF los reportes en el proyecto MiniERP_Suministros.Reports.

# Guía paso a paso para ver reportes en PDF (Crystal Reports)

Esta guía te lleva desde cero hasta descargar los PDF de los dos reportes preparados:
- Ventas por Período y Categoría
- Resumen de Compras por Cliente

Los formularios ya están creados en el sitio y se conectan a la misma base de datos de la solución.

---

## 1) Verificar base de datos lista (una sola vez)
1. Establece como proyecto de inicio: MiniERP_Suministros.Server.
2. Ejecuta el proyecto (F5) una vez. Esto aplica migraciones y, si la BD está vacía, siembra datos demo.
   - Nombre BD: MiniERP_Suministros en (localdb)\\MSSQLLocalDB.
3. Si ya tienes la BD con datos, no hace nada adicional.

---

## 2) (Opcional) Probar el flujo sin Crystal
Ya validaste que se generan TXT/CSV correctamente. Si quieres repetir:
1. Establece como proyecto de inicio: MiniERP_Suministros.Reports.
2. Ejecuta (F5) y entra a:
   - Reportes > Ventas por Período y Categoría
   - Reportes > Resumen de Compras por Cliente
3. Elige un rango que cubra los últimos 6 meses y Exportar (descargará CSV de prueba).

---

## 3) Instalar Crystal Reports para exportar PDF
Crystal no está en NuGet; instálalo en tu equipo de desarrollo:
1. Descarga e instala "SAP Crystal Reports, developer version for Microsoft Visual Studio" (Add-in y Runtime) desde el sitio de SAP.
2. Reinicia Visual Studio al finalizar.
3. Arquitectura recomendada: x64 (usa runtime x64 y compila el proyecto en x64).

---

## 4) Agregar referencias Crystal al proyecto MVC (si no aparecen solas)
1. En MiniERP_Suministros.Reports: Referencias > Agregar Referencia…
2. Agrega:
   - CrystalDecisions.CrystalReports.Engine
   - CrystalDecisions.Shared
   - CrystalDecisions.ReportSource

---

## 5) Habilitar exportación a PDF
1. Propiedades del proyecto (MiniERP_Suministros.Reports) > Compilar.
2. En "Símbolos de compilación condicional" agrega: USE_CRYSTAL
3. Guarda y recompila.

Con esto, las acciones de exportación generarán PDF en lugar de CSV.

---

## 6) Colocar los archivos .rpt en la carpeta Reports
Ruta esperada:
- C:\\Users\\User\\source\\repos\\MiniERP_Suministros_1_0\\MiniERP_Suministros.Reports\\Reports\\

Archivos y definición de datasets/tablas:
1) VentasPorPeriodoYCategoria.rpt
   - Dataset: VentasPeriodoCategoria
   - Tabla: Items
   - Campos (Items): CategoryName, OrderId, OrderDate, CustomerName, ProductName, Quantity, UnitPrice, DetailTotal, OrderDiscount
   - Parámetros: FechaInicio, FechaFin, CategoriaId (opcional), CustomerId (opcional)
   - Diseño sugerido: Grupo por CategoryName y por OrderId; detalle con ProductName, Quantity, UnitPrice, DetailTotal; totales por pedido y categoría; total general.

2) ResumenComprasPorCliente.rpt
   - Dataset: ResumenClientes
   - Tabla: Clientes
   - Campos (Clientes): CustomerId, CustomerName, OrdersCount, OrdersTotal, AvgPerOrder, LastOrderDate
   - Parámetros: FechaInicio, FechaFin, CustomerId (opcional), MontoMinimo (opcional)
   - Diseño sugerido: Grupo por CustomerName; ordenar por OrdersTotal desc; gráfico de barras opcional.

Nota: Puedes diseñar enlazando un .xsd con estas tablas o confiar en el DataSet que pasa la app en tiempo de ejecución.

---

## 7) Ejecutar y descargar PDF
1. Establece MiniERP_Suministros.Reports como proyecto de inicio.
2. Ejecuta (F5) y navega a:
   - /Reports/VentasPeriodoCategoria
   - /Reports/ResumenClientes
3. Selecciona un rango de fechas (últimos 6 meses) y Exportar. Debes obtener un PDF.

---

## 8) Conexión a base de datos (ya configurada)
- Web.config de MiniERP_Suministros.Reports usa:
  - DefaultConnection = Server=(localdb)\\MSSQLLocalDB;Database=MiniERP_Suministros;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true
- No necesitas configurar conexión dentro del .rpt: la app inyecta DataSet.

---

## 9) Rutas útiles en el proyecto
- Formularios de filtros:
  - Views/Reports/VentasPeriodoCategoria.cshtml
  - Views/Reports/ResumenClientes.cshtml
- Controlador:
  - Controllers/ReportsController.cs (acciones ...Export usan Crystal si USE_CRYSTAL está definido)
- Consultas SQL (ADO.NET):
  - Services/ReportsQueryService.cs
- Carpeta de reportes:
  - Reports/ (colocar aquí los .rpt)

---

## 10) Problemas comunes
- Falta de CrystalDecisions.*: instala el add-in/runtime y agrega referencias. Limpia y recompila.
- Conflicto x64/x86: usa runtime Crystal x64 y compila el proyecto en x64.
- PDF vacío: revisa el rango de fechas y que existan datos.
- Conexión: si usas otra instancia, ajusta Web.config > connectionStrings > DefaultConnection.

---

Listo. Con estos pasos podrás colocar los .rpt y descargar los reportes finales en PDF desde el navegador.