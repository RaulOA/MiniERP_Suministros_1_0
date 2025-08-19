RUTA: Copilot-Log-Fixes.md
Descripci�n: Registro de incidencias y soluciones aplicadas por Copilot para consulta futura (wiki t�cnica).

- Incidencia: Errores Angular al servir la app tras crear app-customers-widget.
  - Mensajes:
    - NG8002: "Can't bind to 'attr-title' since it isn't a known property of 'span'".
    - TS-998113: "NgClass is not used within the template of CustomersWidgetComponent" (warning).
  - Contexto: Introducidos al agregar el componente app-customers-widget y su plantilla.
  - Causa ra�z:
    - Uso de attr-title en spans, atributo inv�lido para binding en Angular; debe usarse title.
    - M�dulo NgClass importado sin uso real en la plantilla.
  - Resoluci�n aplicada:
    - Reemplazar attr-title por title en la plantilla del widget.
    - Eliminar NgClass de la lista de imports del componente.
    - Limpieza adicional alineada a la gu�a:
      - Quitar FormsModule (no se usa [(ngModel)] en el widget).
      - Cambiar a inyecci�n por constructor.
      - A�adir namespace de i18n propio customersWidget y mensajes de tabla en ngx-datatable.
      - Alinear propiedad phoneNumber con el backend.
  - Archivos modificados/creados relevantes:
    - src/app/components/widgets/customers-widget.component.html: reemplazo de attr-title por title; mensajes i18n; binding de phoneNumber; messages en ngx-datatable.
    - src/app/components/widgets/customers-widget.component.ts: remoci�n de NgClass/FormsModule de imports; constructor injection; consumo de API; definici�n de columnas con sortable.
    - public/locale/es.json y public/locale/en.json: claves customersWidget.* (management, table, dialog).
    - src/app/models/customer.model.ts: modelo alineado a CustomerVM (id, name, email, phoneNumber...).
    - src/app/services/customers-endpoint.service.ts: endpoints /api/customer (GET/POST/PUT/DELETE).
    - src/app/services/customers.service.ts: servicio de dominio para el widget.
  - Verificaci�n:
    - Compilaci�n correcta posterior a los cambios; errores NG8002 resueltos y warning de NgClass eliminado.

- Incidencia: Error TS2339 en customers-widget.component.ts (getDialogType no existe en AlertService).
  - Mensaje:
    - TS2339: Property 'getDialogType' does not exist on type 'AlertService'.
  - Causa ra�z:
    - Uso de un m�todo inexistente al llamar showStickyMessage/showMessage.
  - Resoluci�n aplicada:
    - Reemplazar el tercer par�metro por el enum MessageSeverity.error proporcionado por AlertService.
    - Importar MessageSeverity desde alert.service.
  - Archivo modificado:
    - src/app/components/widgets/customers-widget.component.ts.
  - Verificaci�n:
    - Compilaci�n correcta tras el cambio; el error TS2339 desaparece.

- Incidencia: NG8002 en customers.component.html por propiedad [verticalScrollbar] no reconocida en <app-customers-widget>.
  - Causa ra�z:
    - CustomersComponent no estaba marcado como standalone, impidiendo el correcto uso del array imports y reconocimiento del input del componente hijo.
  - Resoluci�n aplicada:
    - Se a�adi� standalone: true en el decorador de CustomersComponent.
    - Confirmada la presencia de readonly verticalScrollbar = input(false) en el widget.
  - Verificaci�n:
    - Compilaci�n correcta posterior al ajuste; error NG8002 resuelto.
