# EnglishMatrix — Documentación técnica
> Versión 9 | Aplicación de consola en C# para aprender inglés

---

## Descripción general

**EnglishMatrix** es una aplicación de consola interactiva que ayuda a hispanohablantes a aprender inglés mediante la generación automática de oraciones, quizzes de traducción, conjugación de verbos y lecturas cortas. Toda la interfaz está en español para facilitar el aprendizaje.

---

## Cómo ejecutar

```bash
dotnet new console -n EnglishMatrix
# Reemplaza Program.cs con EnglishMatrix.cs
dotnet run
```

**Requisitos:** .NET SDK (cualquier versión moderna compatible con C# 8+)

---

## Arquitectura

El proyecto es una sola clase estática (`EnglishMatrix`) con los siguientes componentes:

```
EnglishMatrix
├── Modelos de datos
│   ├── Word          — Palabra (sujeto, verbo, lugar, tiempo)
│   ├── Complement    — Complemento directo (comida, objeto, persona…)
│   ├── VerbEntry     — Entrada de verbo con todas sus formas
│   └── SpanishConj   — Conjugaciones en español por persona
├── Datos estáticos
│   ├── conjugaciones — Diccionario de conjugaciones español (~80 verbos)
│   ├── verbList      — Lista de verbos con formas irregulares/regulares
│   ├── verbsForSentences — Verbos usados en la generación de oraciones
│   ├── subjects      — Sujetos disponibles (I, she, we, the hero…)
│   ├── complements   — Objetos directos (comida, bebida, cosas, etc.)
│   ├── places        — Expresiones de lugar (at home, in the city…)
│   └── times         — Expresiones de tiempo (today, every day…)
├── Lógica central
│   ├── BuildEnglishVerb()     — Construye la forma verbal en inglés
│   ├── BuildSpanishVerb()     — Construye la forma verbal en español
│   └── GenerateSentence()     — Genera una oración o pregunta completa
└── Modos de pantalla (Main + 9 modos)
```

---

## Modelos de datos

### `Word`
Representa cualquier palabra que puede ser sujeto, verbo, lugar o tiempo.

| Propiedad   | Tipo     | Descripción                                      |
|-------------|----------|--------------------------------------------------|
| `English`   | `string` | Palabra en inglés                                |
| `Spanish`   | `string` | Traducción al español                            |
| `Category`  | `string` | Categoría: `subject`, `verb`, `place`, `time`   |
| `Accepts`   | `string` | Tipo de complemento que acepta (solo para verbos): `food`, `drink`, `thing`, `person`, `abstract`, `none`, `any` |

### `Complement`
Representa el complemento directo de una oración.

| Propiedad  | Tipo     | Descripción                              |
|------------|----------|------------------------------------------|
| `English`  | `string` | Complemento en inglés                    |
| `Spanish`  | `string` | Traducción al español                    |
| `Type`     | `string` | Tipo: `food`, `drink`, `thing`, `person`, `abstract` |

### `VerbEntry`
Entrada completa de un verbo con todas sus formas morfológicas.

| Propiedad     | Tipo     | Descripción                   |
|---------------|----------|-------------------------------|
| `Infinitive`  | `string` | Forma base (ej. `eat`)        |
| `Past`        | `string` | Pasado simple (ej. `ate`)     |
| `Participle`  | `string` | Participio pasado (ej. `eaten`) |
| `Gerund`      | `string` | Gerundio (ej. `eating`)       |
| `SpanishInf`  | `string` | Infinitivo en español         |
| `IsRegular`   | `bool`   | `true` si el verbo es regular |

### `SpanishConj`
Conjugaciones del verbo en español para las tres personas usadas.

| Propiedad  | Descripción             |
|------------|-------------------------|
| `YoPres`   | Yo — presente           |
| `ElPres`   | Él/Ella — presente      |
| `NosPres`  | Nosotros — presente     |
| `YoPast`   | Yo — pasado             |
| `ElPast`   | Él/Ella — pasado        |
| `NosPast`  | Nosotros — pasado       |

---

## Tiempos verbales soportados (`enum Tense`)

| Valor               | Nombre              | Ejemplo en inglés    |
|---------------------|---------------------|----------------------|
| `PresentSimple`     | Presente simple     | I eat                |
| `PresentContinuous` | Presente continuo   | I am eating          |
| `PastSimple`        | Pasado simple       | I ate                |
| `PastContinuous`    | Pasado continuo     | I was eating         |
| `FutureSimple`      | Futuro simple       | I will eat           |
| `PresentPerfect`    | Presente perfecto   | I have eaten         |
| `Conditional`       | Condicional         | I would eat          |

---

## Lógica central

### `BuildEnglishVerb(string subject, string verb, Tense tense)`
Construye la forma conjugada del verbo en inglés según el sujeto y el tiempo verbal. Maneja correctamente:
- Tercera persona singular (añade `-s`/`-es`/`-ies`)
- Verbos auxiliares (`do/does`, `is/am/are`, `was/were`, `has/have`, `will`, `would`)
- Formas irregulares leídas desde `verbList`

### `BuildSpanishVerb(string subject, string verb, Tense tense)`
Construye la forma conjugada en español. Usa `conjugaciones` (diccionario) para verbos irregulares y genera formas regulares programáticamente para los demás. Cubre:
- Presente, pasado, continuo, perfecto, futuro (`ir a + infinitivo`) y condicional

### `GenerateSentence(bool includePlace, bool includeTime, SentenceMode mode, Tense? forcedTense)`
Motor principal de generación. Selecciona aleatoriamente sujeto, verbo y complemento compatible, agrega opcionalmente lugar y tiempo, y devuelve la tupla `(english, spanish)` con la etiqueta del tiempo verbal.

- **`mode = Normal`** → oración declarativa: `She eats tacos at home.`
- **`mode = Question`** → pregunta con auxiliar correcto: `Does she eat tacos at home?`

---

## Menú principal y modos

| Opción | Función              | Descripción                                                              |
|--------|----------------------|--------------------------------------------------------------------------|
| 1      | `ModeGenerate`       | Genera N oraciones declarativas (tiempo elegido o aleatorio)            |
| 2      | `ModeGenerate`       | Genera N preguntas (mismo motor, modo Question)                         |
| 3      | `ModeQuiz`           | Quiz: muestra oración en inglés, el usuario escribe la traducción       |
| 4      | `ModeQuiz`           | Quiz: igual pero con preguntas                                           |
| 5      | `ModeVerbList`       | Tabla de verbos regulares (infinitivo, pasado, participio, gerundio)    |
| 6      | `ModeVerbList`       | Tabla de verbos irregulares                                              |
| 7      | `ModeVerbQuiz`       | Quiz de verbos regulares (adivina pasado/participio/gerundio)           |
| 8      | `ModeVerbQuiz`       | Quiz de verbos irregulares                                               |
| 9      | `ModeTenseTable`     | Muestra la conjugación completa de un verbo en los 7 tiempos           |
| 10     | `ModeShortStories`   | Lecturas cortas en inglés con traducción                                |
| 11     | `ModeAddWords`       | Agrega sujetos, verbos, complementos, lugares o tiempos personalizados |
| 12     | `ModeListWords`      | Lista todas las palabras cargadas (incluyendo las personalizadas)       |
| 13     | —                    | Salir de la aplicación                                                   |

---

## Modo `ModeAddWords` — Agregar palabras personalizadas

Permite al usuario extender el vocabulario en tiempo de ejecución sin modificar el código. Según la categoría elegida:

- **Verbo**: solicita infinitivo, pasado, participio, gerundio, conjugaciones en español (6 formas) y tipo de complemento que acepta. Agrega la entrada a `verbList`, `verbsForSentences` y `conjugaciones`.
- **Complemento**: solicita la palabra y su tipo semántico, y la agrega a `complements`.
- **Sujeto / Lugar / Tiempo**: agrega la palabra a la lista correspondiente.

> Los datos agregados en sesión no se persisten al disco; se pierden al cerrar la aplicación.

---

## Datos incluidos (resumen)

| Categoría              | Cantidad aproximada |
|------------------------|---------------------|
| Verbos (verbList)      | ~90 verbos          |
| Verbos para oraciones  | ~70 verbos          |
| Conjugaciones español  | ~80 entradas        |
| Sujetos                | ~15                 |
| Complementos           | ~60                 |
| Lugares                | ~20                 |
| Tiempos                | ~15                 |
| Historias cortas       | ~5                  |

---

## Notas de diseño

- El proyecto está contenido en un **único archivo `.cs`** para máxima portabilidad.
- Todos los datos están **embebidos como listas estáticas** (no requiere base de datos ni archivos externos).
- La compatibilidad verbo-complemento se gestiona con el campo `Accepts` en cada verbo, evitando oraciones sin sentido (ej. "I drink a book").
- El generador de preguntas reutiliza el motor de oraciones y aplica inversión auxiliar + sujeto automáticamente.
- Los verbos irregulares del condicional en español tienen tallos especiales (`tendr-`, `pondr-`, etc.) gestionados con un diccionario interno.
