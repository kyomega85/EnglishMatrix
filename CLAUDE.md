# CLAUDE.md

Guía para trabajar en este repositorio.

## Resumen del proyecto

**EnglishMatrix** es una aplicación de consola en C# (.NET 10) que ayuda a hispanohablantes a aprender inglés: genera oraciones y preguntas, quizzes de traducción, tablas de conjugación de verbos y lecturas cortas bilingües. Toda la interfaz de usuario está en español.

**Ejecutar:**
```bash
dotnet run
```

## Arquitectura y convenciones a respetar

- Todo el código vive en `EnglishMatrix.cs`, un único archivo. Mantener esa estructura salvo que se pida explícitamente dividirlo en varios archivos/clases.
- Los datos (verbos, sujetos, complementos, lugares, tiempos, historias) son listas y diccionarios estáticos embebidos en el código. No introducir base de datos ni archivos de datos externos.
- Interfaz de usuario (textos, menús, mensajes) siempre en español. Nombres de variables/métodos en inglés, como el resto del código existente.
- Al agregar o modificar un verbo, debe quedar consistente en las estructuras relacionadas: `verbList`, `conjugaciones` y, si corresponde, `verbsForSentences`. Evitar crear entradas huérfanas (presentes en una estructura pero no en las otras).

## Estado conocido / deuda pendiente

Ver `VERB_ANALYSIS_REPORT.md` para el detalle. En resumen:
- 3 verbos (`laugh`, `cry`, `worry`) existen en `conjugaciones` pero no en `verbList`.
- 35 verbos de `verbList` (incluyendo `be`, `do`, `go`, `have`) no tienen conjugación en español, lo que hace fallar `BuildSpanishVerb()` para esos casos.

No es necesario repetir este análisis; usarlo como contexto de partida al tocar datos de verbos.

## Cómo debe actuar Claude en este repo

- No introducir dependencias externas (paquetes NuGet, frameworks) ni persistencia en disco salvo que se pida explícitamente — el diseño actual es intencionalmente de cero dependencias.
- Verificar los cambios ejecutando `dotnet run` manualmente y probando el flujo de menú afectado, ya que es una app de consola interactiva sin tests automatizados.
- Al tocar verbos o conjugaciones, verificar consistencia cruzada entre `verbList`, `conjugaciones` y `verbsForSentences` antes de dar el cambio por terminado.
- Mantener todos los textos de usuario y comentarios en español, consistente con el resto del código.
