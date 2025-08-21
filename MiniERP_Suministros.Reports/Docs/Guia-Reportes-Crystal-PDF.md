RUTA: MiniERP_Suministros.Reports/Docs/Guia-Reportes-Crystal-PDF.md
Descripción: Guía paso a paso para un usuario sin experiencia previa, para visualizar y exportar a PDF los reportes en el proyecto MiniERP_Suministros.Reports.

# Guía paso a paso para ver reportes en PDF (Crystal Reports)

Esta guía te lleva desde cero hasta descargar los PDF de los dos reportes preparados:
- Ventas por Período y Categoría
- Resumen de Compras por Cliente

Los formularios ya están creados en el sitio y se conectan a la misma base de datos de la solución.

---

## 1) Verificar la base de datos con datos (semillas)
1. Abre la solución en Visual Studio.
2. Establece como proyecto de inicio: MiniERP_Suministros.Server.
3. Ejecuta el proyecto (F5) y espera a que inicie al menos una vez. Esto ejecuta migraciones y siembra datos demo.
   - La base debe llamarse: MiniERP_Suministros (SQL Server local).

Si ya la tenías creada/sembrada, puedes saltar este paso.

---

## 2) Probar el flujo sin Crystal (CSV de prueba)
Antes de instalar Crystal, puedes probar el flujo exportando CSV:
1. Establece como proyecto de inicio: MiniERP_Suministros.Reports.
2. Ejecuta el sitio (F5). Se abrirá la página de inicio.
3. En la página principal, entra a:
   - Reportes > Ventas por Período y Categoría
   - Reportes > Resumen de Compras por Cliente
4. En cada formulario, selecciona un rango amplio (por ejemplo, desde el 1 del mes de hace 6 meses hasta hoy).
5. Clic en Exportar. Se descargará un .csv de prueba. Esto confirma que la conexión a BD y filtros funcionan.

---

## 3) Instalar Crystal Reports para exportar PDF
Crystal no está en NuGet. Debes instalarlo en tu equipo de desarrollo:
1. Descarga e instala "SAP Crystal Reports, developer version for Microsoft Visual Studio" (Add-in y Runtime) desde el sitio oficial de SAP.
2. Cierra y reabre Visual Studio después de instalar.

Sugerencia de plataforma:
- Elige una única arquitectura para todo (x64 recomendado). Instala el runtime x64 y compila el sitio en x64.

---

## 4) Agregar referencias Crystal al proyecto MVC (si el add-in no lo hace solo)
1. En MiniERP_Suministros.Reports, clic derecho en Referencias > Agregar Referencia.
2. Agrega:
   - CrystalDecisions.CrystalReports.Engine
   - CrystalDecisions.Shared
   - CrystalDecisions.ReportSource

Si ya aparecen tras instalar el add-in, no necesitas hacer nada aquí.

---

## 5) Habilitar exportación a PDF en el código
Por defecto, el proyecto exporta CSV. Para habilitar PDF:
1. Propiedades del proyecto (MiniERP_Suministros.Reports) > Compilación.
2. En "Símbolos de compilación condicional" agrega: USE_CRYSTAL
3. Guarda y recompila.

Con esto, las acciones de exportación usarán Crystal y generarán PDF.

---

## 6) Preparar los archivos .rpt
Los reportes deben existir en:
- C:\Users\User\source\repos\MiniERP_Suministros_1_0\MiniERP_Suministros.Reports\Reports\

Crea o copia estos archivos (desde Crystal Reports Designer):
1) VentasPorPeriodoYCategoria.rpt
   - Fuente de datos en diseño: ADO.NET (XML o XSD) o Dataset tipado equivalente.
   - Dataset esperado: VentasPeriodoCategoria con tabla Items.
   - Campos en Items:
     - CategoryName (string)
     - OrderId (int)
     - OrderDate (DateTime)
     - CustomerName (string)
     - ProductName (string)
     - Quantity (int)
     - UnitPrice (decimal)
     - DetailTotal (decimal)
     - OrderDiscount (decimal)
   - Parámetros en el .rpt:
     - FechaInicio (DateTime), FechaFin (DateTime)
     - CategoriaId (Number, opcional), CustomerId (Number, opcional)
   - Diseño sugerido: Grupo 1 por CategoryName, Grupo 2 por OrderId; detalle con ProductName, Quantity, UnitPrice, DetailTotal; totales por pedido y categoría; total general en pie de reporte.

2) ResumenComprasPorCliente.rpt
   - Dataset esperado: ResumenClientes con tabla Clientes.
   - Campos en Clientes:
     - CustomerId (int)
     - CustomerName (string)
     - OrdersCount (int)
     - OrdersTotal (decimal)
     - AvgPerOrder (decimal)
     - LastOrderDate (DateTime)
   - Parámetros en el .rpt:
     - FechaInicio (DateTime), FechaFin (DateTime)
     - CustomerId (Number, opcional), MontoMinimo (Currency, opcional)
   - Diseño sugerido: Grupo por CustomerName; mostrar OrdersCount, OrdersTotal, AvgPerOrder y LastOrderDate; ordenar por OrdersTotal desc; gráfico de barras opcional.

Nota: Si prefieres diseñar con esquema, puedes crear un .xsd con estas tablas y enlazarlo en el diseñador. No es obligatorio, porque el proyecto pasa DataSet con estos nombres en tiempo de ejecución.

---

## 7) Ejecutar y exportar a PDF
1. Establece MiniERP_Suministros.Reports como proyecto de inicio.
2. Ejecuta (F5) y navega a:
   - /Reports/VentasPeriodoCategoria
   - /Reports/ResumenClientes
3. Selecciona rango de fechas (ej.: desde el primer día de hace 6 meses hasta hoy). Los datos demo se distribuyen en los últimos 6 meses.
4. Clic en Exportar. Debes obtener un PDF descargable. Si no definiste USE_CRYSTAL, verás un CSV.

---

## 8) Configuración de conexión (ya lista)
- El proyecto MiniERP_Suministros.Reports ya tiene en Web.config:
  - connectionStrings > DefaultConnection: Server=(local);Database=MiniERP_Suministros;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true
- No necesitas cambiar las conexiones en los .rpt porque el proyecto inyecta DataSet directamente.

---

## 9) Enlaces útiles dentro del proyecto
- Formularios de filtros:
  - Views/Reports/VentasPeriodoCategoria.cshtml
  - Views/Reports/ResumenClientes.cshtml
- Controlador:
  - Controllers/ReportsController.cs (acciones ...Export llaman a Crystal si USE_CRYSTAL)
- Consultas SQL (ADO.NET):
  - Services/ReportsQueryService.cs (mismo esquema que espera cada .rpt)
- Carpeta de reportes:
  - Reports/ (colocar aquí los .rpt)
- Guías rápidas:
  - Reports/ReadMe.txt (datasets/columnas)
  - Docs/Guia-Reportes-Crystal-PDF.md (este archivo)

---

## 10) Problemas comunes y soluciones rápidas
- Error: No se encuentra CrystalDecisions.*
  - Revisa que instalaste el add-in y agregaste referencias. Limpia y recompila.
- Error en servidor x64/x86:
  - Alinea arquitectura: instala runtime Crystal x64 y compila el proyecto como x64 (Propiedades > Compilar > Plataforma de destino).
- PDF vacío o sin datos:
  - Verifica que el rango de fechas cubra los últimos 6 meses y que existan pedidos.
- Cadena de conexión distinta:
  - Ajusta Web.config > connectionStrings > DefaultConnection y prueba de nuevo.

---

Listo. Con estos pasos deberías poder diseñar/colocar los .rpt y descargar los reportes finales en PDF desde el navegador.