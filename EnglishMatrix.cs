// ============================================================
//  EnglishMatrix — Generador de oraciones para aprender inglés
//  v9 — Versión completamente corregida
// ============================================================
// Para correr:
//   dotnet new console -n EnglishMatrix
//   Reemplaza Program.cs con este archivo
//   dotnet run
using System;
using System.Collections.Generic;
using System.Linq;

class EnglishMatrix
{
    // ── Modelos ──────────────────────────────────────────────

    class Word
    {
        public string English  { get; set; }
        public string Spanish  { get; set; }
        public string Category { get; set; }
        public string Accepts  { get; set; } = "any";
    }

    class Complement
    {
        public string English { get; set; }
        public string Spanish { get; set; }
        public string Type    { get; set; }
    }

    class VerbEntry
    {
        public string Infinitive  { get; set; }
        public string Past        { get; set; }
        public string Participle  { get; set; }
        public string Gerund      { get; set; }
        public string SpanishInf  { get; set; }
        public bool   IsRegular   { get; set; }
    }

    class SpanishConj
    {
        public string YoPres  { get; set; }
        public string ElPres  { get; set; }
        public string NosPres { get; set; }
        public string YoPast  { get; set; }
        public string ElPast  { get; set; }
        public string NosPast { get; set; }
    }

    enum Tense
    {
        PresentSimple,      // I eat
        PresentContinuous,  // I am eating
        PastSimple,         // I ate
        PastContinuous,     // I was eating
        FutureSimple,       // I will eat
        PresentPerfect,     // I have eaten
        Conditional         // I would eat
    }

    static string TenseName(Tense t) => t switch
    {
        Tense.PresentSimple     => "Presente simple",
        Tense.PresentContinuous => "Presente continuo",
        Tense.PastSimple        => "Pasado simple",
        Tense.PastContinuous    => "Pasado continuo",
        Tense.FutureSimple      => "Futuro simple",
        Tense.PresentPerfect    => "Presente perfecto",
        Tense.Conditional       => "Condicional",
        _ => ""
    };

    // ── Diccionario de conjugaciones español ─────────────────

    static Dictionary<string, SpanishConj> conjugaciones = new Dictionary<string, SpanishConj>
    {
        { "be",        new SpanishConj { YoPres="soy",           ElPres="es",             NosPres="somos",          YoPast="fui",            ElPast="fue",            NosPast="fuimos"          } },
        { "do",        new SpanishConj { YoPres="hago",          ElPres="hace",           NosPres="hacemos",        YoPast="hice",           ElPast="hizo",           NosPast="hicimos"         } },
        { "go",        new SpanishConj { YoPres="voy",           ElPres="va",             NosPres="vamos",          YoPast="fui",            ElPast="fue",            NosPast="fuimos"          } },
        { "have",      new SpanishConj { YoPres="tengo",         ElPres="tiene",          NosPres="tenemos",        YoPast="tuve",           ElPast="tuvo",           NosPast="tuvimos"         } },
        { "come",      new SpanishConj { YoPres="vengo",         ElPres="viene",          NosPres="venimos",        YoPast="vine",           ElPast="vino",           NosPast="vinimos"         } },
        { "make",      new SpanishConj { YoPres="hago",          ElPres="hace",           NosPres="hacemos",        YoPast="hice",           ElPast="hizo",           NosPast="hicimos"         } },
        { "say",       new SpanishConj { YoPres="digo",          ElPres="dice",           NosPres="decimos",        YoPast="dije",           ElPast="dijo",           NosPast="dijimos"         } },
        { "drink",     new SpanishConj { YoPres="bebo",          ElPres="bebe",           NosPres="bebemos",        YoPast="bebí",           ElPast="bebió",          NosPast="bebimos"         } },
        { "buy",       new SpanishConj { YoPres="compro",        ElPres="compra",         NosPres="compramos",      YoPast="compré",         ElPast="compró",         NosPast="compramos"       } },
        { "need",      new SpanishConj { YoPres="necesito",      ElPres="necesita",       NosPres="necesitamos",    YoPast="necesité",       ElPast="necesitó",       NosPast="necesitamos"     } },
        { "want",      new SpanishConj { YoPres="quiero",        ElPres="quiere",         NosPres="queremos",       YoPast="quise",          ElPast="quiso",          NosPast="quisimos"        } },
        { "help",      new SpanishConj { YoPres="ayudo",         ElPres="ayuda",          NosPres="ayudamos",       YoPast="ayudé",          ElPast="ayudó",          NosPast="ayudamos"        } },
        { "find",      new SpanishConj { YoPres="encuentro",     ElPres="encuentra",      NosPres="encontramos",    YoPast="encontré",       ElPast="encontró",       NosPast="encontramos"     } },
        { "work",      new SpanishConj { YoPres="trabajo",       ElPres="trabaja",        NosPres="trabajamos",     YoPast="trabajé",        ElPast="trabajó",        NosPast="trabajamos"      } },
        { "sleep",     new SpanishConj { YoPres="duermo",        ElPres="duerme",         NosPres="dormimos",       YoPast="dormí",          ElPast="durmió",         NosPast="dormimos"        } },
        { "hide",      new SpanishConj { YoPres="me escondo",    ElPres="se esconde",     NosPres="nos escondemos", YoPast="me escondí",     ElPast="se escondió",    NosPast="nos escondimos"  } },
        { "run",       new SpanishConj { YoPres="corro",         ElPres="corre",          NosPres="corremos",       YoPast="corrí",          ElPast="corrió",         NosPast="corrimos"        } },
        { "jump",      new SpanishConj { YoPres="salto",         ElPres="salta",          NosPres="saltamos",       YoPast="salté",          ElPast="saltó",          NosPast="saltamos"        } },
        { "swim",      new SpanishConj { YoPres="nado",          ElPres="nada",           NosPres="nadamos",        YoPast="nadé",           ElPast="nadó",           NosPast="nadamos"         } },
        { "walk",      new SpanishConj { YoPres="camino",        ElPres="camina",         NosPres="caminamos",      YoPast="caminé",         ElPast="caminó",         NosPast="caminamos"       } },
        { "fight",     new SpanishConj { YoPres="peleo",         ElPres="pelea",          NosPres="peleamos",       YoPast="peleé",          ElPast="peleó",          NosPast="peleamos"        } },
        { "carry",     new SpanishConj { YoPres="cargo",         ElPres="carga",          NosPres="cargamos",       YoPast="cargué",         ElPast="cargó",          NosPast="cargamos"        } },
        { "throw",     new SpanishConj { YoPres="lanzo",         ElPres="lanza",          NosPres="lanzamos",       YoPast="lancé",          ElPast="lanzó",          NosPast="lanzamos"        } },
        { "catch",     new SpanishConj { YoPres="atrapo",        ElPres="atrapa",         NosPres="atrapamos",      YoPast="atrapé",         ElPast="atrapó",         NosPast="atrapamos"       } },
        { "feel",      new SpanishConj { YoPres="siento",        ElPres="siente",         NosPres="sentimos",       YoPast="sentí",          ElPast="sintió",         NosPast="sentimos"        } },
        { "love",      new SpanishConj { YoPres="amo",           ElPres="ama",            NosPres="amamos",         YoPast="amé",            ElPast="amó",            NosPast="amamos"          } },
        { "fear",      new SpanishConj { YoPres="temo",          ElPres="teme",           NosPres="tememos",        YoPast="temí",           ElPast="temió",          NosPast="temimos"         } },
        { "enjoy",     new SpanishConj { YoPres="disfruto",      ElPres="disfruta",       NosPres="disfrutamos",    YoPast="disfruté",       ElPast="disfrutó",       NosPast="disfrutamos"     } },
        { "miss",      new SpanishConj { YoPres="extraño",       ElPres="extraña",        NosPres="extrañamos",     YoPast="extrañé",        ElPast="extrañó",        NosPast="extrañamos"      } },
        { "play",      new SpanishConj { YoPres="juego",         ElPres="juega",          NosPres="jugamos",        YoPast="jugué",          ElPast="jugó",           NosPast="jugamos"         } },
        { "attack",    new SpanishConj { YoPres="ataco",         ElPres="ataca",          NosPres="atacamos",       YoPast="ataqué",         ElPast="atacó",          NosPast="atacamos"        } },
        { "save",      new SpanishConj { YoPres="guardo",        ElPres="guarda",         NosPres="guardamos",      YoPast="guardé",         ElPast="guardó",         NosPast="guardamos"       } },
        { "cook",      new SpanishConj { YoPres="cocino",        ElPres="cocina",         NosPres="cocinamos",      YoPast="cociné",         ElPast="cocinó",         NosPast="cocinamos"       } },
        { "study",     new SpanishConj { YoPres="estudio",       ElPres="estudia",        NosPres="estudiamos",     YoPast="estudié",        ElPast="estudió",        NosPast="estudiamos"      } },
        { "read",      new SpanishConj { YoPres="leo",           ElPres="lee",            NosPres="leemos",         YoPast="leí",            ElPast="leyó",           NosPast="leímos"          } },
        { "write",     new SpanishConj { YoPres="escribo",       ElPres="escribe",        NosPres="escribimos",     YoPast="escribí",        ElPast="escribió",       NosPast="escribimos"      } },
        { "watch",     new SpanishConj { YoPres="veo",           ElPres="ve",             NosPres="vemos",          YoPast="vi",             ElPast="vio",            NosPast="vimos"           } },
        { "call",      new SpanishConj { YoPres="llamo",         ElPres="llama",          NosPres="llamamos",       YoPast="llamé",          ElPast="llamó",          NosPast="llamamos"        } },
        { "open",      new SpanishConj { YoPres="abro",          ElPres="abre",           NosPres="abrimos",        YoPast="abrí",           ElPast="abrió",          NosPast="abrimos"         } },
        { "give",      new SpanishConj { YoPres="doy",           ElPres="da",             NosPres="damos",          YoPast="di",             ElPast="dio",            NosPast="dimos"           } },
        { "take",      new SpanishConj { YoPres="tomo",          ElPres="toma",           NosPres="tomamos",        YoPast="tomé",           ElPast="tomó",           NosPast="tomamos"         } },
        { "use",       new SpanishConj { YoPres="uso",           ElPres="usa",            NosPres="usamos",         YoPast="usé",            ElPast="usó",            NosPast="usamos"          } },
        { "drive",     new SpanishConj { YoPres="manejo",        ElPres="maneja",         NosPres="manejamos",      YoPast="manejé",         ElPast="manejó",         NosPast="manejamos"       } },
        { "clean",     new SpanishConj { YoPres="limpio",        ElPres="limpia",         NosPres="limpiamos",      YoPast="limpié",         ElPast="limpió",         NosPast="limpiamos"       } },
        { "install",   new SpanishConj { YoPres="instalo",       ElPres="instala",        NosPres="instalamos",     YoPast="instalé",        ElPast="instaló",        NosPast="instalamos"      } },
        { "upgrade",   new SpanishConj { YoPres="mejoro",        ElPres="mejora",         NosPres="mejoramos",      YoPast="mejoré",         ElPast="mejoró",         NosPast="mejoramos"       } },
        { "unlock",    new SpanishConj { YoPres="desbloqueo",    ElPres="desbloquea",     NosPres="desbloqueamos",  YoPast="desbloqueé",     ElPast="desbloqueó",     NosPast="desbloqueamos"   } },
        { "hack",      new SpanishConj { YoPres="hackeo",        ElPres="hackea",         NosPres="hackeamos",      YoPast="hackeé",         ElPast="hackeó",         NosPast="hackeamos"       } },
        { "defend",    new SpanishConj { YoPres="defiendo",      ElPres="defiende",       NosPres="defendemos",     YoPast="defendí",        ElPast="defendió",       NosPast="defendimos"      } },
        { "listen",    new SpanishConj { YoPres="escucho",       ElPres="escucha",        NosPres="escuchamos",     YoPast="escuché",        ElPast="escuchó",        NosPast="escuchamos"      } },
        { "push",      new SpanishConj { YoPres="empujo",        ElPres="empuja",         NosPres="empujamos",      YoPast="empujé",         ElPast="empujó",         NosPast="empujamos"       } },
        { "trust",     new SpanishConj { YoPres="confío en",     ElPres="confía en",      NosPres="confiamos en",   YoPast="confié en",      ElPast="confió en",      NosPast="confiamos en"    } },
        { "ask",       new SpanishConj { YoPres="pregunto",      ElPres="pregunta",       NosPres="preguntamos",    YoPast="pregunté",       ElPast="preguntó",       NosPast="preguntamos"     } },
        { "answer",    new SpanishConj { YoPres="respondo",      ElPres="responde",       NosPres="respondemos",    YoPast="respondí",       ElPast="respondió",      NosPast="respondimos"     } },
        { "start",     new SpanishConj { YoPres="empiezo",       ElPres="empieza",        NosPres="empezamos",      YoPast="empecé",         ElPast="empezó",         NosPast="empezamos"       } },
        { "stop",      new SpanishConj { YoPres="paro",          ElPres="para",           NosPres="paramos",        YoPast="paré",           ElPast="paró",           NosPast="paramos"         } },
        { "finish",    new SpanishConj { YoPres="termino",       ElPres="termina",        NosPres="terminamos",     YoPast="terminé",        ElPast="terminó",        NosPast="terminamos"      } },
        { "try",       new SpanishConj { YoPres="intento",       ElPres="intenta",        NosPres="intentamos",     YoPast="intenté",        ElPast="intentó",        NosPast="intentamos"      } },
        { "fail",      new SpanishConj { YoPres="fallo",         ElPres="falla",          NosPres="fallamos",       YoPast="fallé",          ElPast="falló",          NosPast="fallamos"        } },
        { "win",       new SpanishConj { YoPres="gano",          ElPres="gana",           NosPres="ganamos",        YoPast="gané",           ElPast="ganó",           NosPast="ganamos"         } },
        { "lose",      new SpanishConj { YoPres="pierdo",        ElPres="pierde",         NosPres="perdemos",       YoPast="perdí",          ElPast="perdió",         NosPast="perdimos"        } },
        { "learn",     new SpanishConj { YoPres="aprendo",       ElPres="aprende",        NosPres="aprendemos",     YoPast="aprendí",        ElPast="aprendió",       NosPast="aprendimos"      } },
        { "teach",     new SpanishConj { YoPres="enseño",        ElPres="enseña",         NosPres="enseñamos",      YoPast="enseñé",         ElPast="enseñó",         NosPast="enseñamos"       } },
        { "show",      new SpanishConj { YoPres="muestro",       ElPres="muestra",        NosPres="mostramos",      YoPast="mostré",         ElPast="mostró",         NosPast="mostramos"       } },
        { "tell",      new SpanishConj { YoPres="digo",          ElPres="dice",           NosPres="decimos",        YoPast="dije",           ElPast="dijo",           NosPast="dijimos"         } },
        { "speak",     new SpanishConj { YoPres="hablo",         ElPres="habla",          NosPres="hablamos",       YoPast="hablé",          ElPast="habló",          NosPast="hablamos"        } },
        { "hear",      new SpanishConj { YoPres="oigo",          ElPres="oye",            NosPres="oímos",          YoPast="oí",             ElPast="oyó",            NosPast="oímos"           } },
        { "see",       new SpanishConj { YoPres="veo",           ElPres="ve",             NosPres="vemos",          YoPast="vi",             ElPast="vio",            NosPast="vimos"           } },
        { "look",      new SpanishConj { YoPres="miro",          ElPres="mira",           NosPres="miramos",        YoPast="miré",           ElPast="miró",           NosPast="miramos"         } },
        { "wait",      new SpanishConj { YoPres="espero",        ElPres="espera",         NosPres="esperamos",      YoPast="esperé",         ElPast="esperó",         NosPast="esperamos"       } },
        { "move",      new SpanishConj { YoPres="muevo",         ElPres="mueve",          NosPres="movemos",        YoPast="moví",           ElPast="movió",          NosPast="movimos"         } },
        { "turn",      new SpanishConj { YoPres="giro",          ElPres="gira",           NosPres="giramos",        YoPast="giré",           ElPast="giró",           NosPast="giramos"         } },
        { "pull",      new SpanishConj { YoPres="jalo",          ElPres="jala",           NosPres="jalamos",        YoPast="jalé",           ElPast="jaló",           NosPast="jalamos"         } },
        { "drop",      new SpanishConj { YoPres="suelto",        ElPres="suelta",         NosPres="soltamos",       YoPast="solté",          ElPast="soltó",          NosPast="soltamos"        } },
        { "build",     new SpanishConj { YoPres="construyo",     ElPres="construye",      NosPres="construimos",    YoPast="construí",       ElPast="construyó",      NosPast="construimos"     } },
        { "break",     new SpanishConj { YoPres="rompo",         ElPres="rompe",          NosPres="rompemos",       YoPast="rompí",          ElPast="rompió",         NosPast="rompimos"        } },
        { "fix",       new SpanishConj { YoPres="arreglo",       ElPres="arregla",        NosPres="arreglamos",     YoPast="arreglé",        ElPast="arregló",        NosPast="arreglamos"      } },
        { "create",    new SpanishConj { YoPres="creo",          ElPres="crea",           NosPres="creamos",        YoPast="creé",           ElPast="creó",           NosPast="creamos"         } },
        { "destroy",   new SpanishConj { YoPres="destruyo",      ElPres="destruye",       NosPres="destruimos",     YoPast="destruí",        ElPast="destruyó",       NosPast="destruimos"      } },
        { "send",      new SpanishConj { YoPres="envío",         ElPres="envía",          NosPres="enviamos",       YoPast="envié",          ElPast="envió",          NosPast="enviamos"        } },
        { "receive",   new SpanishConj { YoPres="recibo",        ElPres="recibe",         NosPres="recibimos",      YoPast="recibí",         ElPast="recibió",        NosPast="recibimos"       } },
        { "pay",       new SpanishConj { YoPres="pago",          ElPres="paga",           NosPres="pagamos",        YoPast="pagué",          ElPast="pagó",           NosPast="pagamos"         } },
        { "sell",      new SpanishConj { YoPres="vendo",         ElPres="vende",          NosPres="vendemos",       YoPast="vendí",          ElPast="vendió",         NosPast="vendimos"        } },
        { "spend",     new SpanishConj { YoPres="gasto",         ElPres="gasta",          NosPres="gastamos",       YoPast="gasté",          ElPast="gastó",          NosPast="gastamos"        } },
        { "choose",    new SpanishConj { YoPres="elijo",         ElPres="elige",          NosPres="elegimos",       YoPast="elegí",          ElPast="eligió",         NosPast="elegimos"        } },
        { "decide",    new SpanishConj { YoPres="decido",        ElPres="decide",         NosPres="decidimos",      YoPast="decidí",         ElPast="decidió",        NosPast="decidimos"       } },
        { "think",     new SpanishConj { YoPres="pienso",        ElPres="piensa",         NosPres="pensamos",       YoPast="pensé",          ElPast="pensó",          NosPast="pensamos"        } },
        { "know",      new SpanishConj { YoPres="sé",            ElPres="sabe",           NosPres="sabemos",        YoPast="supe",           ElPast="supo",           NosPast="supimos"         } },
        { "remember",  new SpanishConj { YoPres="recuerdo",      ElPres="recuerda",       NosPres="recordamos",     YoPast="recordé",        ElPast="recordó",        NosPast="recordamos"      } },
        { "forget",    new SpanishConj { YoPres="olvido",        ElPres="olvida",         NosPres="olvidamos",      YoPast="olvidé",         ElPast="olvidó",         NosPast="olvidamos"       } },
        { "understand",new SpanishConj { YoPres="entiendo",      ElPres="entiende",       NosPres="entendemos",     YoPast="entendí",        ElPast="entendió",       NosPast="entendimos"      } },
        { "explain",   new SpanishConj { YoPres="explico",       ElPres="explica",        NosPres="explicamos",     YoPast="expliqué",       ElPast="explicó",        NosPast="explicamos"      } },
        { "suggest",   new SpanishConj { YoPres="sugiero",       ElPres="sugiere",        NosPres="sugerimos",      YoPast="sugerí",         ElPast="sugirió",        NosPast="sugerimos"       } },
        { "accept",    new SpanishConj { YoPres="acepto",        ElPres="acepta",         NosPres="aceptamos",      YoPast="acepté",         ElPast="aceptó",         NosPast="aceptamos"       } },
        { "refuse",    new SpanishConj { YoPres="rechazo",       ElPres="rechaza",        NosPres="rechazamos",     YoPast="rechacé",        ElPast="rechazó",        NosPast="rechazamos"      } },
        { "visit",     new SpanishConj { YoPres="visito",        ElPres="visita",         NosPres="visitamos",      YoPast="visité",         ElPast="visitó",         NosPast="visitamos"       } },
        { "meet",      new SpanishConj { YoPres="conozco",       ElPres="conoce",         NosPres="conocemos",      YoPast="conocí",         ElPast="conoció",        NosPast="conocimos"       } },
        { "leave",     new SpanishConj { YoPres="salgo",         ElPres="sale",           NosPres="salimos",        YoPast="salí",           ElPast="salió",          NosPast="salimos"         } },
        { "arrive",    new SpanishConj { YoPres="llego",         ElPres="llega",          NosPres="llegamos",       YoPast="llegué",         ElPast="llegó",          NosPast="llegamos"        } },
        { "return",    new SpanishConj { YoPres="regreso",       ElPres="regresa",        NosPres="regresamos",     YoPast="regresé",        ElPast="regresó",        NosPast="regresamos"      } },
        { "travel",    new SpanishConj { YoPres="viajo",         ElPres="viaja",          NosPres="viajamos",       YoPast="viajé",          ElPast="viajó",          NosPast="viajamos"        } },
        { "climb",     new SpanishConj { YoPres="escalo",        ElPres="escala",         NosPres="escalamos",      YoPast="escalé",         ElPast="escaló",         NosPast="escalamos"       } },
        { "fall",      new SpanishConj { YoPres="caigo",         ElPres="cae",            NosPres="caemos",         YoPast="caí",            ElPast="cayó",           NosPast="caímos"          } },
        { "fly",       new SpanishConj { YoPres="vuelo",         ElPres="vuela",          NosPres="volamos",        YoPast="volé",           ElPast="voló",           NosPast="volamos"         } },
        { "grow",      new SpanishConj { YoPres="crezco",        ElPres="crece",          NosPres="crecemos",       YoPast="crecí",          ElPast="creció",         NosPast="crecimos"        } },
        { "change",    new SpanishConj { YoPres="cambio",        ElPres="cambia",         NosPres="cambiamos",      YoPast="cambié",         ElPast="cambió",         NosPast="cambiamos"       } },
        { "add",       new SpanishConj { YoPres="agrego",        ElPres="agrega",         NosPres="agregamos",      YoPast="agregué",        ElPast="agregó",         NosPast="agregamos"       } },
        { "remove",    new SpanishConj { YoPres="quito",         ElPres="quita",          NosPres="quitamos",       YoPast="quité",          ElPast="quitó",          NosPast="quitamos"        } },
        { "check",     new SpanishConj { YoPres="reviso",        ElPres="revisa",         NosPres="revisamos",      YoPast="revisé",         ElPast="revisó",         NosPast="revisamos"       } },
        { "test",      new SpanishConj { YoPres="pruebo",        ElPres="prueba",         NosPres="probamos",       YoPast="probé",          ElPast="probó",          NosPast="probamos"        } },
        { "connect",   new SpanishConj { YoPres="conecto",       ElPres="conecta",        NosPres="conectamos",     YoPast="conecté",        ElPast="conectó",        NosPast="conectamos"      } },
        { "download",  new SpanishConj { YoPres="descargo",      ElPres="descarga",       NosPres="descargamos",    YoPast="descargué",      ElPast="descargó",       NosPast="descargamos"     } },
        { "upload",    new SpanishConj { YoPres="subo",          ElPres="sube",           NosPres="subimos",        YoPast="subí",           ElPast="subió",          NosPast="subimos"         } },
        { "share",     new SpanishConj { YoPres="comparto",      ElPres="comparte",       NosPres="compartimos",    YoPast="compartí",       ElPast="compartió",      NosPast="compartimos"     } },
        { "search",    new SpanishConj { YoPres="busco",         ElPres="busca",          NosPres="buscamos",       YoPast="busqué",         ElPast="buscó",          NosPast="buscamos"        } },
        { "type",      new SpanishConj { YoPres="escribo",       ElPres="escribe",        NosPres="escribimos",     YoPast="escribí",        ElPast="escribió",       NosPast="escribimos"      } },
        { "print",     new SpanishConj { YoPres="imprimo",       ElPres="imprime",        NosPres="imprimimos",     YoPast="imprimí",        ElPast="imprimió",       NosPast="imprimimos"      } },
        { "copy",      new SpanishConj { YoPres="copio",         ElPres="copia",          NosPres="copiamos",       YoPast="copié",          ElPast="copió",          NosPast="copiamos"        } },
        { "delete",    new SpanishConj { YoPres="elimino",       ElPres="elimina",        NosPres="eliminamos",     YoPast="eliminé",        ElPast="eliminó",        NosPast="eliminamos"      } },
        { "close",     new SpanishConj { YoPres="cierro",        ElPres="cierra",         NosPres="cerramos",       YoPast="cerré",          ElPast="cerró",          NosPast="cerramos"        } },
        { "wake",      new SpanishConj { YoPres="despierto",     ElPres="despierta",      NosPres="despertamos",    YoPast="desperté",       ElPast="despertó",       NosPast="despertamos"     } },
        { "dress",     new SpanishConj { YoPres="me visto",      ElPres="se viste",       NosPres="nos vestimos",   YoPast="me vestí",       ElPast="se vistió",      NosPast="nos vestimos"    } },
        { "wash",      new SpanishConj { YoPres="lavo",          ElPres="lava",           NosPres="lavamos",        YoPast="lavé",           ElPast="lavó",           NosPast="lavamos"         } },
        { "eat",       new SpanishConj { YoPres="como",          ElPres="come",           NosPres="comemos",        YoPast="comí",           ElPast="comió",          NosPast="comimos"         } },
        { "sing",      new SpanishConj { YoPres="canto",         ElPres="canta",          NosPres="cantamos",       YoPast="canté",          ElPast="cantó",          NosPast="cantamos"        } },
        { "dance",     new SpanishConj { YoPres="bailo",         ElPres="baila",          NosPres="bailamos",       YoPast="bailé",          ElPast="bailó",          NosPast="bailamos"        } },
        { "draw",      new SpanishConj { YoPres="dibujo",        ElPres="dibuja",         NosPres="dibujamos",      YoPast="dibujé",         ElPast="dibujó",         NosPast="dibujamos"       } },
        { "paint",     new SpanishConj { YoPres="pinto",         ElPres="pinta",          NosPres="pintamos",       YoPast="pinté",          ElPast="pintó",          NosPast="pintamos"        } },
        { "laugh",     new SpanishConj { YoPres="río",           ElPres="ríe",            NosPres="reímos",         YoPast="reí",            ElPast="rió",            NosPast="reímos"          } },
        { "cry",       new SpanishConj { YoPres="lloro",         ElPres="llora",          NosPres="lloramos",       YoPast="lloré",          ElPast="lloró",          NosPast="lloramos"        } },
        { "worry",     new SpanishConj { YoPres="me preocupo",   ElPres="se preocupa",    NosPres="nos preocupamos",YoPast="me preocupé",    ElPast="se preocupó",    NosPast="nos preocupamos" } },
        { "celebrate", new SpanishConj { YoPres="celebro",       ElPres="celebra",        NosPres="celebramos",     YoPast="celebré",        ElPast="celebró",        NosPast="celebramos"      } },
        { "practice",  new SpanishConj { YoPres="practico",      ElPres="practica",       NosPres="practicamos",    YoPast="practiqué",      ElPast="practicó",       NosPast="practicamos"     } },
        { "protect",   new SpanishConj { YoPres="protejo",       ElPres="protege",        NosPres="protegemos",     YoPast="protegí",        ElPast="protegió",       NosPast="protegimos"      } },
        { "escape",    new SpanishConj { YoPres="escapo",        ElPres="escapa",         NosPres="escapamos",      YoPast="escapé",         ElPast="escapó",         NosPast="escapamos"       } },
        { "rescue",    new SpanishConj { YoPres="rescato",       ElPres="rescata",        NosPres="rescatamos",     YoPast="rescaté",        ElPast="rescató",        NosPast="rescatamos"      } },
        { "explore",   new SpanishConj { YoPres="exploro",       ElPres="explora",        NosPres="exploramos",     YoPast="exploré",        ElPast="exploró",        NosPast="exploramos"      } },
        { "collect",   new SpanishConj { YoPres="colecciono",    ElPres="colecciona",     NosPres="coleccionamos",  YoPast="coleccioné",     ElPast="coleccionó",     NosPast="coleccionamos"   } },
        { "complete",  new SpanishConj { YoPres="completo",      ElPres="completa",       NosPres="completamos",    YoPast="completé",       ElPast="completó",       NosPast="completamos"     } },
        { "activate",  new SpanishConj { YoPres="activo",        ElPres="activa",         NosPres="activamos",      YoPast="activé",         ElPast="activó",         NosPast="activamos"       } },
        { "beat",      new SpanishConj { YoPres="golpeo",        ElPres="golpea",         NosPres="golpeamos",      YoPast="golpeé",         ElPast="golpeó",         NosPast="golpeamos"       } },
        { "become",    new SpanishConj { YoPres="me hago",       ElPres="se hace",        NosPres="nos hacemos",    YoPast="me hice",        ElPast="se hizo",        NosPast="nos hicimos"     } },
        { "begin",     new SpanishConj { YoPres="comienzo",      ElPres="comienza",       NosPres="comenzamos",     YoPast="comencé",        ElPast="comenzó",        NosPast="comenzamos"      } },
        { "blow",      new SpanishConj { YoPres="soplo",         ElPres="sopla",          NosPres="soplamos",       YoPast="soplé",          ElPast="sopló",          NosPast="soplamos"        } },
        { "bring",     new SpanishConj { YoPres="traigo",        ElPres="trae",           NosPres="traemos",        YoPast="traje",          ElPast="trajo",          NosPast="trajimos"        } },
        { "cost",      new SpanishConj { YoPres="cuesta",        ElPres="cuesta",         NosPres="cuesta",         YoPast="costó",          ElPast="costó",          NosPast="costó"           } },
        { "cut",       new SpanishConj { YoPres="corto",         ElPres="corta",          NosPres="cortamos",       YoPast="corté",          ElPast="cortó",          NosPast="cortamos"        } },
        { "get",       new SpanishConj { YoPres="obtengo",       ElPres="obtiene",        NosPres="obtenemos",      YoPast="obtuve",         ElPast="obtuvo",         NosPast="obtuvimos"       } },
        { "hold",      new SpanishConj { YoPres="sostengo",      ElPres="sostiene",       NosPres="sostenemos",     YoPast="sostuve",        ElPast="sostuvo",        NosPast="sostuvimos"      } },
        { "hurt",      new SpanishConj { YoPres="lastimo",       ElPres="lastima",        NosPres="lastimamos",     YoPast="lastimé",        ElPast="lastimó",        NosPast="lastimamos"      } },
        { "keep",      new SpanishConj { YoPres="mantengo",      ElPres="mantiene",       NosPres="mantenemos",     YoPast="mantuve",        ElPast="mantuvo",        NosPast="mantuvimos"      } },
        { "lead",      new SpanishConj { YoPres="lidero",        ElPres="lidera",         NosPres="lideramos",      YoPast="lideré",         ElPast="lideró",         NosPast="lideramos"       } },
        { "lend",      new SpanishConj { YoPres="presto",        ElPres="presta",         NosPres="prestamos",      YoPast="presté",         ElPast="prestó",         NosPast="prestamos"       } },
        { "let",       new SpanishConj { YoPres="dejo",          ElPres="deja",           NosPres="dejamos",        YoPast="dejé",           ElPast="dejó",           NosPast="dejamos"         } },
        { "mean",      new SpanishConj { YoPres="significo",     ElPres="significa",      NosPres="significamos",   YoPast="significé",      ElPast="significó",      NosPast="significamos"    } },
        { "put",       new SpanishConj { YoPres="pongo",         ElPres="pone",           NosPres="ponemos",        YoPast="puse",           ElPast="puso",           NosPast="pusimos"         } },
        { "ride",      new SpanishConj { YoPres="monto",         ElPres="monta",          NosPres="montamos",       YoPast="monté",          ElPast="montó",          NosPast="montamos"        } },
        { "ring",      new SpanishConj { YoPres="toco",          ElPres="toca",           NosPres="tocamos",        YoPast="toqué",          ElPast="tocó",           NosPast="tocamos"         } },
        { "rise",      new SpanishConj { YoPres="me levanto",    ElPres="se levanta",     NosPres="nos levantamos", YoPast="me levanté",     ElPast="se levantó",     NosPast="nos levantamos"  } },
        { "seek",      new SpanishConj { YoPres="busco",         ElPres="busca",          NosPres="buscamos",       YoPast="busqué",         ElPast="buscó",          NosPast="buscamos"        } },
        { "set",       new SpanishConj { YoPres="pongo",         ElPres="pone",           NosPres="ponemos",        YoPast="puse",           ElPast="puso",           NosPast="pusimos"         } },
        { "shake",     new SpanishConj { YoPres="sacudo",        ElPres="sacude",         NosPres="sacudimos",      YoPast="sacudí",         ElPast="sacudió",        NosPast="sacudimos"       } },
        { "shoot",     new SpanishConj { YoPres="disparo",       ElPres="dispara",        NosPres="disparamos",     YoPast="disparé",        ElPast="disparó",        NosPast="disparamos"      } },
        { "shrink",    new SpanishConj { YoPres="me encojo",     ElPres="se encoge",      NosPres="nos encogemos",  YoPast="me encogí",      ElPast="se encogio",     NosPast="nos encogimos"   } },
        { "shut",      new SpanishConj { YoPres="cierro",        ElPres="cierra",         NosPres="cerramos",       YoPast="cerré",          ElPast="cerró",          NosPast="cerramos"        } },
        { "sink",      new SpanishConj { YoPres="me hundo",      ElPres="se hunde",       NosPres="nos hundimos",   YoPast="me hundí",       ElPast="se hundió",      NosPast="nos hundimos"    } },
        { "stand",     new SpanishConj { YoPres="me levanto",    ElPres="se levanta",     NosPres="nos levantamos", YoPast="me levanté",     ElPast="se levantó",     NosPast="nos levantamos"  } },
        { "steal",     new SpanishConj { YoPres="robo",          ElPres="roba",           NosPres="robamos",        YoPast="robé",           ElPast="robó",           NosPast="robamos"         } },
        { "strike",    new SpanishConj { YoPres="golpeo",        ElPres="golpea",         NosPres="golpeamos",      YoPast="golpeé",         ElPast="golpeó",         NosPast="golpeamos"       } },
        { "swing",     new SpanishConj { YoPres="columpio",      ElPres="columpia",       NosPres="columpiamos",    YoPast="columpié",       ElPast="columpió",       NosPast="columpiamos"     } },
        { "wear",      new SpanishConj { YoPres="uso/llevo",     ElPres="usa/lleva",      NosPres="usamos/llevamos",YoPast="usé/llevé",      ElPast="usó/llevó",      NosPast="usamos/llevamos"}},
        { "load",      new SpanishConj { YoPres="cargo",         ElPres="carga",          NosPres="cargamos",       YoPast="cargué",         ElPast="cargó",          NosPast="cargamos"        } },
        { "smile",     new SpanishConj { YoPres="sonrío",        ElPres="sonríe",         NosPres="sonreímos",      YoPast="sonreí",         ElPast="sonrió",         NosPast="sonreímos"       } },
        { "invite",    new SpanishConj { YoPres="invito",        ElPres="invita",         NosPres="invitamos",      YoPast="invité",         ElPast="invitó",         NosPast="invitamos"       } },
        { "live",      new SpanishConj { YoPres="vivo",          ElPres="vive",           NosPres="vivimos",        YoPast="viví",           ElPast="vivió",          NosPast="vivimos"         } },
        { "talk",      new SpanishConj { YoPres="hablo",         ElPres="habla",          NosPres="hablamos",       YoPast="hablé",          ElPast="habló",          NosPast="hablamos"        } },
        { "like",      new SpanishConj { YoPres="gusto",         ElPres="gusta",          NosPres="gustamos",       YoPast="gusté",          ElPast="gustó",          NosPast="gustamos"        } },
        { "order",     new SpanishConj { YoPres="ordeno",        ElPres="ordena",         NosPres="ordenamos",      YoPast="ordené",         ElPast="ordenó",         NosPast="ordenamos"       } },
        { "plan",      new SpanishConj { YoPres="planeo",        ElPres="planea",         NosPres="planeamos",      YoPast="planeé",         ElPast="planeó",         NosPast="planeamos"       } },
        { "rain",      new SpanishConj { YoPres="lluevo",        ElPres="llueve",         NosPres="llovemos",       YoPast="lloví",          ElPast="llovió",         NosPast="llovimos"        } },
        { "shop",      new SpanishConj { YoPres="compro",        ElPres="compra",         NosPres="compramos",      YoPast="compré",         ElPast="compró",         NosPast="compramos"       } },
        { "snow",      new SpanishConj { YoPres="nievo",         ElPres="nieva",          NosPres="nevamos",        YoPast="nevó",           ElPast="nevó",           NosPast="nevó"            } },
        { "exercise",  new SpanishConj { YoPres="ejercito",      ElPres="ejercita",       NosPres="ejercitamos",    YoPast="ejercité",       ElPast="ejercitó",       NosPast="ejercitamos"     } },
        { "brush",     new SpanishConj { YoPres="cepillo",       ElPres="cepilla",        NosPres="cepillamos",     YoPast="cepillé",        ElPast="cepilló",        NosPast="cepillamos"      } },
        { "comb",      new SpanishConj { YoPres="peino",         ElPres="peina",          NosPres="peinamos",       YoPast="peiné",          ElPast="peinó",          NosPast="peinamos"        } },
        { "agree",     new SpanishConj { YoPres="acuerdo",       ElPres="acuerda",        NosPres="acordamos",      YoPast="acordé",         ElPast="acordó",         NosPast="acordamos"       } },
        { "borrow",    new SpanishConj { YoPres="pido prestado", ElPres="pide prestado",  NosPres="pedimos prestado",YoPast="pedí prestado",  ElPast="pidió prestado", NosPast="pedimos prestado" } },
        { "rent",      new SpanishConj { YoPres="alquilo",       ElPres="alquila",        NosPres="alquilamos",     YoPast="alquilé",        ElPast="alquiló",        NosPast="alquilamos"      } },
        { "rest",      new SpanishConj { YoPres="descanso",      ElPres="descansa",       NosPres="descansamos",    YoPast="descansé",       ElPast="descansó",       NosPast="descansamos"     } },
        { "stay",      new SpanishConj { YoPres="quedo",         ElPres="queda",          NosPres="quedamos",       YoPast="quedé",          ElPast="quedó",          NosPast="quedamos"        } },
    };

    // ── Lista completa de verbos (SIN DUPLICADOS) ──
    
    static List<VerbEntry> verbList = new List<VerbEntry>
    {
        // REGULARES
        new VerbEntry { Infinitive="accept",    Past="accepted",    Participle="accepted",    Gerund="accepting",    SpanishInf="aceptar",      IsRegular=true },
        new VerbEntry { Infinitive="activate",  Past="activated",   Participle="activated",   Gerund="activating",   SpanishInf="activar",      IsRegular=true },
        new VerbEntry { Infinitive="add",       Past="added",       Participle="added",       Gerund="adding",       SpanishInf="agregar",      IsRegular=true },
        new VerbEntry { Infinitive="agree",     Past="agreed",      Participle="agreed",      Gerund="agreeing",     SpanishInf="acordar",      IsRegular=true },
        new VerbEntry { Infinitive="answer",    Past="answered",    Participle="answered",    Gerund="answering",    SpanishInf="responder",    IsRegular=true },
        new VerbEntry { Infinitive="arrive",    Past="arrived",     Participle="arrived",     Gerund="arriving",     SpanishInf="llegar",       IsRegular=true },
        new VerbEntry { Infinitive="ask",       Past="asked",       Participle="asked",       Gerund="asking",       SpanishInf="preguntar",    IsRegular=true },
        new VerbEntry { Infinitive="attack",    Past="attacked",    Participle="attacked",    Gerund="attacking",    SpanishInf="atacar",       IsRegular=true },
        new VerbEntry { Infinitive="borrow",    Past="borrowed",    Participle="borrowed",    Gerund="borrowing",    SpanishInf="pedir prestado", IsRegular=true },
        new VerbEntry { Infinitive="brush",     Past="brushed",     Participle="brushed",     Gerund="brushing",     SpanishInf="cepillar",     IsRegular=true },
        new VerbEntry { Infinitive="call",      Past="called",      Participle="called",      Gerund="calling",      SpanishInf="llamar",       IsRegular=true },
        new VerbEntry { Infinitive="carry",     Past="carried",     Participle="carried",     Gerund="carrying",     SpanishInf="cargar",       IsRegular=true },
        new VerbEntry { Infinitive="celebrate", Past="celebrated",  Participle="celebrated",  Gerund="celebrating",  SpanishInf="celebrar",     IsRegular=true },
        new VerbEntry { Infinitive="change",    Past="changed",     Participle="changed",     Gerund="changing",     SpanishInf="cambiar",      IsRegular=true },
        new VerbEntry { Infinitive="check",     Past="checked",     Participle="checked",     Gerund="checking",     SpanishInf="revisar",      IsRegular=true },
        new VerbEntry { Infinitive="clean",     Past="cleaned",     Participle="cleaned",     Gerund="cleaning",     SpanishInf="limpiar",      IsRegular=true },
        new VerbEntry { Infinitive="climb",     Past="climbed",     Participle="climbed",     Gerund="climbing",     SpanishInf="escalar",      IsRegular=true },
        new VerbEntry { Infinitive="close",     Past="closed",      Participle="closed",      Gerund="closing",      SpanishInf="cerrar",       IsRegular=true },
        new VerbEntry { Infinitive="collect",   Past="collected",   Participle="collected",   Gerund="collecting",   SpanishInf="coleccionar",  IsRegular=true },
        new VerbEntry { Infinitive="comb",      Past="combed",      Participle="combed",      Gerund="combing",      SpanishInf="peinar",       IsRegular=true },
        new VerbEntry { Infinitive="complete",  Past="completed",   Participle="completed",   Gerund="completing",   SpanishInf="completar",    IsRegular=true },
        new VerbEntry { Infinitive="connect",   Past="connected",   Participle="connected",   Gerund="connecting",   SpanishInf="conectar",     IsRegular=true },
        new VerbEntry { Infinitive="cook",      Past="cooked",      Participle="cooked",      Gerund="cooking",      SpanishInf="cocinar",      IsRegular=true },
        new VerbEntry { Infinitive="copy",      Past="copied",      Participle="copied",      Gerund="copying",      SpanishInf="copiar",       IsRegular=true },
        new VerbEntry { Infinitive="create",    Past="created",     Participle="created",     Gerund="creating",     SpanishInf="crear",        IsRegular=true },
        new VerbEntry { Infinitive="cry",       Past="cried",       Participle="cried",       Gerund="crying",       SpanishInf="llorar",       IsRegular=true },
        new VerbEntry { Infinitive="dance",     Past="danced",      Participle="danced",      Gerund="dancing",      SpanishInf="bailar",       IsRegular=true },
        new VerbEntry { Infinitive="decide",    Past="decided",     Participle="decided",     Gerund="deciding",     SpanishInf="decidir",      IsRegular=true },
        new VerbEntry { Infinitive="defend",    Past="defended",    Participle="defended",    Gerund="defending",    SpanishInf="defender",     IsRegular=true },
        new VerbEntry { Infinitive="delete",    Past="deleted",     Participle="deleted",     Gerund="deleting",     SpanishInf="eliminar",     IsRegular=true },
        new VerbEntry { Infinitive="download",  Past="downloaded",  Participle="downloaded",  Gerund="downloading",  SpanishInf="descargar",    IsRegular=true },
        new VerbEntry { Infinitive="dress",     Past="dressed",     Participle="dressed",     Gerund="dressing",     SpanishInf="vestirse",     IsRegular=true },
        new VerbEntry { Infinitive="drop",      Past="dropped",     Participle="dropped",     Gerund="dropping",     SpanishInf="soltar",       IsRegular=true },
        new VerbEntry { Infinitive="enjoy",     Past="enjoyed",     Participle="enjoyed",     Gerund="enjoying",     SpanishInf="disfrutar",    IsRegular=true },
        new VerbEntry { Infinitive="escape",    Past="escaped",     Participle="escaped",     Gerund="escaping",     SpanishInf="escapar",      IsRegular=true },
        new VerbEntry { Infinitive="exercise",  Past="exercised",   Participle="exercised",   Gerund="exercising",   SpanishInf="ejercitar",    IsRegular=true },
        new VerbEntry { Infinitive="explain",   Past="explained",   Participle="explained",   Gerund="explaining",   SpanishInf="explicar",     IsRegular=true },
        new VerbEntry { Infinitive="explore",   Past="explored",    Participle="explored",    Gerund="exploring",    SpanishInf="explorar",     IsRegular=true },
        new VerbEntry { Infinitive="fail",      Past="failed",      Participle="failed",      Gerund="failing",      SpanishInf="fallar",       IsRegular=true },
        new VerbEntry { Infinitive="finish",    Past="finished",    Participle="finished",    Gerund="finishing",    SpanishInf="terminar",     IsRegular=true },
        new VerbEntry { Infinitive="fix",       Past="fixed",       Participle="fixed",       Gerund="fixing",       SpanishInf="arreglar",     IsRegular=true },
        new VerbEntry { Infinitive="hack",      Past="hacked",      Participle="hacked",      Gerund="hacking",      SpanishInf="hackear",      IsRegular=true },
        new VerbEntry { Infinitive="help",      Past="helped",      Participle="helped",      Gerund="helping",      SpanishInf="ayudar",       IsRegular=true },
        new VerbEntry { Infinitive="install",   Past="installed",   Participle="installed",   Gerund="installing",   SpanishInf="instalar",     IsRegular=true },
        new VerbEntry { Infinitive="invite",    Past="invited",     Participle="invited",     Gerund="inviting",     SpanishInf="invitar",      IsRegular=true },
        new VerbEntry { Infinitive="jump",      Past="jumped",      Participle="jumped",      Gerund="jumping",      SpanishInf="saltar",       IsRegular=true },
        new VerbEntry { Infinitive="laugh",     Past="laughed",     Participle="laughed",     Gerund="laughing",     SpanishInf="reír",         IsRegular=true },
        new VerbEntry { Infinitive="learn",     Past="learned",     Participle="learned",     Gerund="learning",     SpanishInf="aprender",     IsRegular=true },
        new VerbEntry { Infinitive="like",      Past="liked",       Participle="liked",       Gerund="liking",       SpanishInf="gustar",       IsRegular=true },
        new VerbEntry { Infinitive="listen",    Past="listened",    Participle="listened",    Gerund="listening",    SpanishInf="escuchar",     IsRegular=true },
        new VerbEntry { Infinitive="live",      Past="lived",       Participle="lived",       Gerund="living",       SpanishInf="vivir",        IsRegular=true },
        new VerbEntry { Infinitive="load",      Past="loaded",      Participle="loaded",      Gerund="loading",      SpanishInf="cargar",       IsRegular=true },
        new VerbEntry { Infinitive="look",      Past="looked",      Participle="looked",      Gerund="looking",      SpanishInf="mirar",        IsRegular=true },
        new VerbEntry { Infinitive="love",      Past="loved",       Participle="loved",       Gerund="loving",       SpanishInf="amar",         IsRegular=true },
        new VerbEntry { Infinitive="miss",      Past="missed",      Participle="missed",      Gerund="missing",      SpanishInf="extrañar",     IsRegular=true },
        new VerbEntry { Infinitive="move",      Past="moved",       Participle="moved",       Gerund="moving",       SpanishInf="mover",        IsRegular=true },
        new VerbEntry { Infinitive="need",      Past="needed",      Participle="needed",      Gerund="needing",      SpanishInf="necesitar",    IsRegular=true },
        new VerbEntry { Infinitive="open",      Past="opened",      Participle="opened",      Gerund="opening",      SpanishInf="abrir",        IsRegular=true },
        new VerbEntry { Infinitive="order",     Past="ordered",     Participle="ordered",     Gerund="ordering",     SpanishInf="ordenar",      IsRegular=true },
        new VerbEntry { Infinitive="paint",     Past="painted",     Participle="painted",     Gerund="painting",     SpanishInf="pintar",       IsRegular=true },
        new VerbEntry { Infinitive="pay",       Past="paid",        Participle="paid",        Gerund="paying",       SpanishInf="pagar",        IsRegular=true },
        new VerbEntry { Infinitive="plan",      Past="planned",     Participle="planned",     Gerund="planning",     SpanishInf="planear",      IsRegular=true },
        new VerbEntry { Infinitive="play",      Past="played",      Participle="played",      Gerund="playing",      SpanishInf="jugar",        IsRegular=true },
        new VerbEntry { Infinitive="practice",  Past="practiced",   Participle="practiced",   Gerund="practicing",   SpanishInf="practicar",    IsRegular=true },
        new VerbEntry { Infinitive="print",     Past="printed",     Participle="printed",     Gerund="printing",     SpanishInf="imprimir",     IsRegular=true },
        new VerbEntry { Infinitive="protect",   Past="protected",   Participle="protected",   Gerund="protecting",   SpanishInf="proteger",     IsRegular=true },
        new VerbEntry { Infinitive="pull",      Past="pulled",      Participle="pulled",      Gerund="pulling",      SpanishInf="jalar",        IsRegular=true },
        new VerbEntry { Infinitive="push",      Past="pushed",      Participle="pushed",      Gerund="pushing",      SpanishInf="empujar",      IsRegular=true },
        new VerbEntry { Infinitive="rain",      Past="rained",      Participle="rained",      Gerund="raining",      SpanishInf="llover",       IsRegular=true },
        new VerbEntry { Infinitive="receive",   Past="received",    Participle="received",    Gerund="receiving",    SpanishInf="recibir",      IsRegular=true },
        new VerbEntry { Infinitive="refuse",    Past="refused",     Participle="refused",     Gerund="refusing",     SpanishInf="rechazar",     IsRegular=true },
        new VerbEntry { Infinitive="remember",  Past="remembered",  Participle="remembered",  Gerund="remembering",  SpanishInf="recordar",     IsRegular=true },
        new VerbEntry { Infinitive="remove",    Past="removed",     Participle="removed",     Gerund="removing",     SpanishInf="quitar",       IsRegular=true },
        new VerbEntry { Infinitive="rent",      Past="rented",      Participle="rented",      Gerund="renting",      SpanishInf="alquilar",     IsRegular=true },
        new VerbEntry { Infinitive="rescue",    Past="rescued",     Participle="rescued",     Gerund="rescuing",     SpanishInf="rescatar",     IsRegular=true },
        new VerbEntry { Infinitive="rest",      Past="rested",      Participle="rested",      Gerund="resting",      SpanishInf="descansar",    IsRegular=true },
        new VerbEntry { Infinitive="return",    Past="returned",    Participle="returned",    Gerund="returning",    SpanishInf="regresar",     IsRegular=true },
        new VerbEntry { Infinitive="save",      Past="saved",       Participle="saved",       Gerund="saving",       SpanishInf="guardar",      IsRegular=true },
        new VerbEntry { Infinitive="search",    Past="searched",    Participle="searched",    Gerund="searching",    SpanishInf="buscar",       IsRegular=true },
        new VerbEntry { Infinitive="share",     Past="shared",      Participle="shared",      Gerund="sharing",      SpanishInf="compartir",    IsRegular=true },
        new VerbEntry { Infinitive="shop",      Past="shopped",     Participle="shopped",     Gerund="shopping",     SpanishInf="comprar",      IsRegular=true },
        new VerbEntry { Infinitive="show",      Past="showed",      Participle="shown",       Gerund="showing",      SpanishInf="mostrar",      IsRegular=true },
        new VerbEntry { Infinitive="smile",     Past="smiled",      Participle="smiled",      Gerund="smiling",      SpanishInf="sonreír",      IsRegular=true },
        new VerbEntry { Infinitive="snow",      Past="snowed",      Participle="snowed",      Gerund="snowing",      SpanishInf="nevar",        IsRegular=true },
        new VerbEntry { Infinitive="start",     Past="started",     Participle="started",     Gerund="starting",     SpanishInf="empezar",      IsRegular=true },
        new VerbEntry { Infinitive="stay",      Past="stayed",      Participle="stayed",      Gerund="staying",      SpanishInf="quedarse",     IsRegular=true },
        new VerbEntry { Infinitive="stop",      Past="stopped",     Participle="stopped",     Gerund="stopping",     SpanishInf="parar",        IsRegular=true },
        new VerbEntry { Infinitive="study",     Past="studied",     Participle="studied",     Gerund="studying",     SpanishInf="estudiar",     IsRegular=true },
        new VerbEntry { Infinitive="suggest",   Past="suggested",   Participle="suggested",   Gerund="suggesting",   SpanishInf="sugerir",      IsRegular=true },
        new VerbEntry { Infinitive="talk",      Past="talked",      Participle="talked",      Gerund="talking",      SpanishInf="hablar",       IsRegular=true },
        new VerbEntry { Infinitive="test",      Past="tested",      Participle="tested",      Gerund="testing",      SpanishInf="probar",       IsRegular=true },
        new VerbEntry { Infinitive="travel",    Past="traveled",    Participle="traveled",    Gerund="traveling",    SpanishInf="viajar",       IsRegular=true },
        new VerbEntry { Infinitive="try",       Past="tried",       Participle="tried",       Gerund="trying",       SpanishInf="intentar",     IsRegular=true },
        new VerbEntry { Infinitive="turn",      Past="turned",      Participle="turned",      Gerund="turning",      SpanishInf="girar",        IsRegular=true },
        new VerbEntry { Infinitive="type",      Past="typed",       Participle="typed",       Gerund="typing",       SpanishInf="escribir",     IsRegular=true },
        new VerbEntry { Infinitive="unlock",    Past="unlocked",    Participle="unlocked",    Gerund="unlocking",    SpanishInf="desbloquear",  IsRegular=true },
        new VerbEntry { Infinitive="upgrade",   Past="upgraded",    Participle="upgraded",    Gerund="upgrading",    SpanishInf="mejorar",      IsRegular=true },
        new VerbEntry { Infinitive="upload",    Past="uploaded",    Participle="uploaded",    Gerund="uploading",    SpanishInf="subir",        IsRegular=true },
        new VerbEntry { Infinitive="use",       Past="used",        Participle="used",        Gerund="using",        SpanishInf="usar",         IsRegular=true },
        new VerbEntry { Infinitive="visit",     Past="visited",     Participle="visited",     Gerund="visiting",     SpanishInf="visitar",      IsRegular=true },
        new VerbEntry { Infinitive="wait",      Past="waited",      Participle="waited",      Gerund="waiting",      SpanishInf="esperar",      IsRegular=true },
        new VerbEntry { Infinitive="walk",      Past="walked",      Participle="walked",      Gerund="walking",      SpanishInf="caminar",      IsRegular=true },
        new VerbEntry { Infinitive="want",      Past="wanted",      Participle="wanted",      Gerund="wanting",      SpanishInf="querer",       IsRegular=true },
        new VerbEntry { Infinitive="wash",      Past="washed",      Participle="washed",      Gerund="washing",      SpanishInf="lavar",        IsRegular=true },
        new VerbEntry { Infinitive="watch",     Past="watched",     Participle="watched",     Gerund="watching",     SpanishInf="ver",          IsRegular=true },
        new VerbEntry { Infinitive="work",      Past="worked",      Participle="worked",      Gerund="working",      SpanishInf="trabajar",     IsRegular=true },
        new VerbEntry { Infinitive="worry",     Past="worried",     Participle="worried",     Gerund="worrying",     SpanishInf="preocuparse",  IsRegular=true },

        // IRREGULARES
        new VerbEntry { Infinitive="be",        Past="was/were",    Participle="been",        Gerund="being",        SpanishInf="ser/estar",    IsRegular=false },
        new VerbEntry { Infinitive="beat",      Past="beat",        Participle="beaten",      Gerund="beating",      SpanishInf="golpear",      IsRegular=false },
        new VerbEntry { Infinitive="become",    Past="became",      Participle="become",      Gerund="becoming",     SpanishInf="convertirse",  IsRegular=false },
        new VerbEntry { Infinitive="begin",     Past="began",       Participle="begun",       Gerund="beginning",    SpanishInf="comenzar",     IsRegular=false },
        new VerbEntry { Infinitive="blow",      Past="blew",        Participle="blown",       Gerund="blowing",      SpanishInf="soplar",       IsRegular=false },
        new VerbEntry { Infinitive="break",     Past="broke",       Participle="broken",      Gerund="breaking",     SpanishInf="romper",       IsRegular=false },
        new VerbEntry { Infinitive="bring",     Past="brought",     Participle="brought",     Gerund="bringing",     SpanishInf="traer",        IsRegular=false },
        new VerbEntry { Infinitive="build",     Past="built",       Participle="built",       Gerund="building",     SpanishInf="construir",    IsRegular=false },
        new VerbEntry { Infinitive="buy",       Past="bought",      Participle="bought",      Gerund="buying",       SpanishInf="comprar",      IsRegular=false },
        new VerbEntry { Infinitive="catch",     Past="caught",      Participle="caught",      Gerund="catching",     SpanishInf="atrapar",      IsRegular=false },
        new VerbEntry { Infinitive="choose",    Past="chose",       Participle="chosen",      Gerund="choosing",     SpanishInf="elegir",       IsRegular=false },
        new VerbEntry { Infinitive="come",      Past="came",        Participle="come",        Gerund="coming",       SpanishInf="venir",        IsRegular=false },
        new VerbEntry { Infinitive="cost",      Past="cost",        Participle="cost",        Gerund="costing",      SpanishInf="costar",       IsRegular=false },
        new VerbEntry { Infinitive="cut",       Past="cut",         Participle="cut",         Gerund="cutting",      SpanishInf="cortar",       IsRegular=false },
        new VerbEntry { Infinitive="do",        Past="did",         Participle="done",        Gerund="doing",        SpanishInf="hacer",        IsRegular=false },
        new VerbEntry { Infinitive="draw",      Past="drew",        Participle="drawn",       Gerund="drawing",      SpanishInf="dibujar",      IsRegular=false },
        new VerbEntry { Infinitive="drink",     Past="drank",       Participle="drunk",       Gerund="drinking",     SpanishInf="beber",        IsRegular=false },
        new VerbEntry { Infinitive="drive",     Past="drove",       Participle="driven",      Gerund="driving",      SpanishInf="manejar",      IsRegular=false },
        new VerbEntry { Infinitive="eat",       Past="ate",         Participle="eaten",       Gerund="eating",       SpanishInf="comer",        IsRegular=false },
        new VerbEntry { Infinitive="fall",      Past="fell",        Participle="fallen",      Gerund="falling",      SpanishInf="caer",         IsRegular=false },
        new VerbEntry { Infinitive="feel",      Past="felt",        Participle="felt",        Gerund="feeling",      SpanishInf="sentir",       IsRegular=false },
        new VerbEntry { Infinitive="fight",     Past="fought",      Participle="fought",      Gerund="fighting",     SpanishInf="pelear",       IsRegular=false },
        new VerbEntry { Infinitive="find",      Past="found",       Participle="found",       Gerund="finding",      SpanishInf="encontrar",    IsRegular=false },
        new VerbEntry { Infinitive="fly",       Past="flew",        Participle="flown",       Gerund="flying",       SpanishInf="volar",        IsRegular=false },
        new VerbEntry { Infinitive="forget",    Past="forgot",      Participle="forgotten",   Gerund="forgetting",   SpanishInf="olvidar",      IsRegular=false },
        new VerbEntry { Infinitive="get",       Past="got",         Participle="gotten",      Gerund="getting",      SpanishInf="obtener",      IsRegular=false },
        new VerbEntry { Infinitive="give",      Past="gave",        Participle="given",       Gerund="giving",       SpanishInf="dar",          IsRegular=false },
        new VerbEntry { Infinitive="go",        Past="went",        Participle="gone",        Gerund="going",        SpanishInf="ir",           IsRegular=false },
        new VerbEntry { Infinitive="grow",      Past="grew",        Participle="grown",       Gerund="growing",      SpanishInf="crecer",       IsRegular=false },
        new VerbEntry { Infinitive="hang",      Past="hung",        Participle="hung",        Gerund="hanging",      SpanishInf="colgar",       IsRegular=false },
        new VerbEntry { Infinitive="have",      Past="had",         Participle="had",         Gerund="having",       SpanishInf="tener",        IsRegular=false },
        new VerbEntry { Infinitive="hear",      Past="heard",       Participle="heard",       Gerund="hearing",      SpanishInf="oír",          IsRegular=false },
        new VerbEntry { Infinitive="hide",      Past="hid",         Participle="hidden",      Gerund="hiding",       SpanishInf="esconderse",   IsRegular=false },
        new VerbEntry { Infinitive="hit",       Past="hit",         Participle="hit",         Gerund="hitting",      SpanishInf="golpear",      IsRegular=false },
        new VerbEntry { Infinitive="hold",      Past="held",        Participle="held",        Gerund="holding",      SpanishInf="sostener",     IsRegular=false },
        new VerbEntry { Infinitive="hurt",      Past="hurt",        Participle="hurt",        Gerund="hurting",      SpanishInf="lastimar",     IsRegular=false },
        new VerbEntry { Infinitive="keep",      Past="kept",        Participle="kept",        Gerund="keeping",      SpanishInf="mantener",     IsRegular=false },
        new VerbEntry { Infinitive="know",      Past="knew",        Participle="known",       Gerund="knowing",      SpanishInf="saber",        IsRegular=false },
        new VerbEntry { Infinitive="lead",      Past="led",         Participle="led",         Gerund="leading",      SpanishInf="liderar",      IsRegular=false },
        new VerbEntry { Infinitive="leave",     Past="left",        Participle="left",        Gerund="leaving",      SpanishInf="salir",        IsRegular=false },
        new VerbEntry { Infinitive="lend",      Past="lent",        Participle="lent",        Gerund="lending",      SpanishInf="prestar",      IsRegular=false },
        new VerbEntry { Infinitive="let",       Past="let",         Participle="let",         Gerund="letting",      SpanishInf="dejar",        IsRegular=false },
        new VerbEntry { Infinitive="lose",      Past="lost",        Participle="lost",        Gerund="losing",       SpanishInf="perder",       IsRegular=false },
        new VerbEntry { Infinitive="make",      Past="made",        Participle="made",        Gerund="making",       SpanishInf="hacer",        IsRegular=false },
        new VerbEntry { Infinitive="mean",      Past="meant",       Participle="meant",       Gerund="meaning",      SpanishInf="significar",   IsRegular=false },
        new VerbEntry { Infinitive="meet",      Past="met",         Participle="met",         Gerund="meeting",      SpanishInf="conocer",      IsRegular=false },
        new VerbEntry { Infinitive="put",       Past="put",         Participle="put",         Gerund="putting",      SpanishInf="poner",        IsRegular=false },
        new VerbEntry { Infinitive="read",      Past="read",        Participle="read",        Gerund="reading",      SpanishInf="leer",         IsRegular=false },
        new VerbEntry { Infinitive="ride",      Past="rode",        Participle="ridden",      Gerund="riding",       SpanishInf="montar",       IsRegular=false },
        new VerbEntry { Infinitive="ring",      Past="rang",        Participle="rung",        Gerund="ringing",      SpanishInf="sonar",        IsRegular=false },
        new VerbEntry { Infinitive="rise",      Past="rose",        Participle="risen",       Gerund="rising",       SpanishInf="subir",        IsRegular=false },
        new VerbEntry { Infinitive="run",       Past="ran",         Participle="run",         Gerund="running",      SpanishInf="correr",       IsRegular=false },
        new VerbEntry { Infinitive="say",       Past="said",        Participle="said",        Gerund="saying",       SpanishInf="decir",        IsRegular=false },
        new VerbEntry { Infinitive="see",       Past="saw",         Participle="seen",        Gerund="seeing",       SpanishInf="ver",          IsRegular=false },
        new VerbEntry { Infinitive="seek",      Past="sought",      Participle="sought",      Gerund="seeking",      SpanishInf="buscar",       IsRegular=false },
        new VerbEntry { Infinitive="sell",      Past="sold",        Participle="sold",        Gerund="selling",      SpanishInf="vender",       IsRegular=false },
        new VerbEntry { Infinitive="send",      Past="sent",        Participle="sent",        Gerund="sending",      SpanishInf="enviar",       IsRegular=false },
        new VerbEntry { Infinitive="set",       Past="set",         Participle="set",         Gerund="setting",      SpanishInf="establecer",   IsRegular=false },
        new VerbEntry { Infinitive="shake",     Past="shook",       Participle="shaken",      Gerund="shaking",      SpanishInf="sacudir",      IsRegular=false },
        new VerbEntry { Infinitive="shoot",     Past="shot",        Participle="shot",        Gerund="shooting",     SpanishInf="disparar",     IsRegular=false },
        new VerbEntry { Infinitive="shrink",    Past="shrank",      Participle="shrunk",      Gerund="shrinking",    SpanishInf="encoger",      IsRegular=false },
        new VerbEntry { Infinitive="shut",      Past="shut",        Participle="shut",        Gerund="shutting",     SpanishInf="cerrar",       IsRegular=false },
        new VerbEntry { Infinitive="sing",      Past="sang",        Participle="sung",        Gerund="singing",      SpanishInf="cantar",       IsRegular=false },
        new VerbEntry { Infinitive="sink",      Past="sank",        Participle="sunk",        Gerund="sinking",      SpanishInf="hundir",       IsRegular=false },
        new VerbEntry { Infinitive="sit",       Past="sat",         Participle="sat",         Gerund="sitting",      SpanishInf="sentarse",     IsRegular=false },
        new VerbEntry { Infinitive="sleep",     Past="slept",       Participle="slept",       Gerund="sleeping",     SpanishInf="dormir",       IsRegular=false },
        new VerbEntry { Infinitive="speak",     Past="spoke",       Participle="spoken",      Gerund="speaking",     SpanishInf="hablar",       IsRegular=false },
        new VerbEntry { Infinitive="spend",     Past="spent",       Participle="spent",       Gerund="spending",     SpanishInf="gastar",       IsRegular=false },
        new VerbEntry { Infinitive="stand",     Past="stood",       Participle="stood",       Gerund="standing",     SpanishInf="pararse",      IsRegular=false },
        new VerbEntry { Infinitive="steal",     Past="stole",       Participle="stolen",      Gerund="stealing",     SpanishInf="robar",        IsRegular=false },
        new VerbEntry { Infinitive="strike",    Past="struck",      Participle="struck",      Gerund="striking",     SpanishInf="golpear",      IsRegular=false },
        new VerbEntry { Infinitive="swim",      Past="swam",        Participle="swum",        Gerund="swimming",     SpanishInf="nadar",        IsRegular=false },
        new VerbEntry { Infinitive="swing",     Past="swung",       Participle="swung",       Gerund="swinging",     SpanishInf="balancearse",  IsRegular=false },
        new VerbEntry { Infinitive="take",      Past="took",        Participle="taken",       Gerund="taking",       SpanishInf="tomar",        IsRegular=false },
        new VerbEntry { Infinitive="teach",     Past="taught",      Participle="taught",      Gerund="teaching",     SpanishInf="enseñar",      IsRegular=false },
        new VerbEntry { Infinitive="tear",      Past="tore",        Participle="torn",        Gerund="tearing",      SpanishInf="rasgar",       IsRegular=false },
        new VerbEntry { Infinitive="tell",      Past="told",        Participle="told",        Gerund="telling",      SpanishInf="decir",        IsRegular=false },
        new VerbEntry { Infinitive="think",     Past="thought",     Participle="thought",     Gerund="thinking",     SpanishInf="pensar",       IsRegular=false },
        new VerbEntry { Infinitive="throw",     Past="threw",       Participle="thrown",      Gerund="throwing",     SpanishInf="lanzar",       IsRegular=false },
        new VerbEntry { Infinitive="understand",Past="understood",  Participle="understood",  Gerund="understanding",SpanishInf="entender",     IsRegular=false },
        new VerbEntry { Infinitive="wake",      Past="woke",        Participle="woken",       Gerund="waking",       SpanishInf="despertar",    IsRegular=false },
        new VerbEntry { Infinitive="wear",      Past="wore",        Participle="worn",        Gerund="wearing",      SpanishInf="usar/llevar",  IsRegular=false },
        new VerbEntry { Infinitive="win",       Past="won",         Participle="won",         Gerund="winning",      SpanishInf="ganar",        IsRegular=false },
        new VerbEntry { Infinitive="write",     Past="wrote",       Participle="written",     Gerund="writing",      SpanishInf="escribir",     IsRegular=false },
    };

    // ── Banco de sujetos, complementos, lugares, tiempos ─────

    static List<Word> subjects = new List<Word>
    {
        new Word { English = "I",                  Spanish = "Yo",                 Category = "subject" },
        new Word { English = "You",                Spanish = "Tú",                 Category = "subject" },
        new Word { English = "She",                Spanish = "Ella",               Category = "subject" },
        new Word { English = "He",                 Spanish = "Él",                 Category = "subject" },
        new Word { English = "We",                 Spanish = "Nosotros",           Category = "subject" },
        new Word { English = "They",               Spanish = "Ellos/Ellas",        Category = "subject" },
        new Word { English = "My mom",             Spanish = "Mi mamá",            Category = "subject" },
        new Word { English = "My dad",             Spanish = "Mi papá",            Category = "subject" },
        new Word { English = "My friend",          Spanish = "Mi amigo",           Category = "subject" },
        new Word { English = "My sister",          Spanish = "Mi hermana",         Category = "subject" },
        new Word { English = "My brother",         Spanish = "Mi hermano",         Category = "subject" },
        new Word { English = "The teacher",        Spanish = "El maestro/La maestra", Category = "subject" },
        new Word { English = "The student",        Spanish = "El estudiante",      Category = "subject" },
        new Word { English = "The cat",            Spanish = "El gato",            Category = "subject" },
        new Word { English = "The dog",            Spanish = "El perro",           Category = "subject" },
        new Word { English = "The bird",           Spanish = "El pájaro",          Category = "subject" },
        new Word { English = "The fish",           Spanish = "El pez",             Category = "subject" },
        new Word { English = "Snake",              Spanish = "La serpiente",       Category = "subject" },
        new Word { English = "Mario",              Spanish = "Mario",              Category = "subject" },
        new Word { English = "Link",               Spanish = "Link",               Category = "subject" },
        new Word { English = "The soldier",        Spanish = "El soldado",         Category = "subject" },
        new Word { English = "The player",         Spanish = "El jugador",         Category = "subject" },
    };

    static List<Word> verbsForSentences = new List<Word>
    {
        new Word { English = "eat",       Spanish = "comer",        Category = "verb", Accepts = "food"     },
        new Word { English = "drink",     Spanish = "beber",        Category = "verb", Accepts = "drink"    },
        new Word { English = "buy",       Spanish = "comprar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "need",      Spanish = "necesitar",    Category = "verb", Accepts = "abstract" },
        new Word { English = "want",      Spanish = "querer",       Category = "verb", Accepts = "abstract" },
        new Word { English = "help",      Spanish = "ayudar",       Category = "verb", Accepts = "person"   },
        new Word { English = "find",      Spanish = "encontrar",    Category = "verb", Accepts = "thing"    },
        new Word { English = "give",      Spanish = "dar",          Category = "verb", Accepts = "thing"    },
        new Word { English = "take",      Spanish = "tomar",        Category = "verb", Accepts = "thing"    },
        new Word { English = "use",       Spanish = "usar",         Category = "verb", Accepts = "thing"    },
        new Word { English = "save",      Spanish = "guardar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "read",      Spanish = "leer",         Category = "verb", Accepts = "thing"    },
        new Word { English = "write",     Spanish = "escribir",     Category = "verb", Accepts = "thing"    },
        new Word { English = "watch",     Spanish = "ver",          Category = "verb", Accepts = "thing"    },
        new Word { English = "call",      Spanish = "llamar",       Category = "verb", Accepts = "person"   },
        new Word { English = "open",      Spanish = "abrir",        Category = "verb", Accepts = "thing"    },
        new Word { English = "enjoy",     Spanish = "disfrutar",    Category = "verb", Accepts = "thing"    },
        new Word { English = "catch",     Spanish = "atrapar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "throw",     Spanish = "lanzar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "carry",     Spanish = "cargar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "love",      Spanish = "amar",         Category = "verb", Accepts = "person"   },
        new Word { English = "fear",      Spanish = "temer",        Category = "verb", Accepts = "person"   },
        new Word { English = "miss",      Spanish = "extrañar",     Category = "verb", Accepts = "person"   },
        new Word { English = "fight",     Spanish = "pelear",       Category = "verb", Accepts = "person"   },
        new Word { English = "attack",    Spanish = "atacar",       Category = "verb", Accepts = "person"   },
        new Word { English = "defend",    Spanish = "defender",     Category = "verb", Accepts = "person"   },
        new Word { English = "play",      Spanish = "jugar",        Category = "verb", Accepts = "thing"    },
        new Word { English = "upgrade",   Spanish = "mejorar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "unlock",    Spanish = "desbloquear",  Category = "verb", Accepts = "thing"    },
        new Word { English = "hack",      Spanish = "hackear",      Category = "verb", Accepts = "thing"    },
        new Word { English = "install",   Spanish = "instalar",     Category = "verb", Accepts = "thing"    },
        new Word { English = "cook",      Spanish = "cocinar",      Category = "verb", Accepts = "food"     },
        new Word { English = "clean",     Spanish = "limpiar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "drive",     Spanish = "manejar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "study",     Spanish = "estudiar",     Category = "verb", Accepts = "abstract" },
        new Word { English = "listen",    Spanish = "escuchar",     Category = "verb", Accepts = "thing"    },
        new Word { English = "feel",      Spanish = "sentir",       Category = "verb", Accepts = "abstract" },
        new Word { English = "build",     Spanish = "construir",    Category = "verb", Accepts = "thing"    },
        new Word { English = "break",     Spanish = "romper",       Category = "verb", Accepts = "thing"    },
        new Word { English = "fix",       Spanish = "arreglar",     Category = "verb", Accepts = "thing"    },
        new Word { English = "send",      Spanish = "enviar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "sell",      Spanish = "vender",       Category = "verb", Accepts = "thing"    },
        new Word { English = "choose",    Spanish = "elegir",       Category = "verb", Accepts = "thing"    },
        new Word { English = "teach",     Spanish = "enseñar",      Category = "verb", Accepts = "person"   },
        new Word { English = "show",      Spanish = "mostrar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "tell",      Spanish = "decir",        Category = "verb", Accepts = "person"   },
        new Word { English = "ask",       Spanish = "preguntar",    Category = "verb", Accepts = "person"   },
        new Word { English = "meet",      Spanish = "conocer",      Category = "verb", Accepts = "person"   },
        new Word { English = "protect",   Spanish = "proteger",     Category = "verb", Accepts = "person"   },
        new Word { English = "escape",    Spanish = "escapar",      Category = "verb", Accepts = "none"     },
        new Word { English = "explore",   Spanish = "explorar",     Category = "verb", Accepts = "none"     },
        new Word { English = "work",      Spanish = "trabajar",     Category = "verb", Accepts = "none"     },
        new Word { English = "sleep",     Spanish = "dormir",       Category = "verb", Accepts = "none"     },
        new Word { English = "hide",      Spanish = "esconderse",   Category = "verb", Accepts = "none"     },
        new Word { English = "run",       Spanish = "correr",       Category = "verb", Accepts = "none"     },
        new Word { English = "jump",      Spanish = "saltar",       Category = "verb", Accepts = "none"     },
        new Word { English = "swim",      Spanish = "nadar",        Category = "verb", Accepts = "none"     },
        new Word { English = "walk",      Spanish = "caminar",      Category = "verb", Accepts = "none"     },
        new Word { English = "fly",       Spanish = "volar",        Category = "verb", Accepts = "none"     },
        new Word { English = "fall",      Spanish = "caer",         Category = "verb", Accepts = "none"     },
        new Word { English = "laugh",     Spanish = "reírse",       Category = "verb", Accepts = "none"     },
        new Word { English = "cry",       Spanish = "llorar",       Category = "verb", Accepts = "none"     },
        new Word { English = "sing",      Spanish = "cantar",       Category = "verb", Accepts = "none"     },
        new Word { English = "dance",     Spanish = "bailar",       Category = "verb", Accepts = "none"     },
        new Word { English = "brush",     Spanish = "cepillar",     Category = "verb", Accepts = "thing"    },
        new Word { English = "comb",      Spanish = "peinar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "exercise",  Spanish = "ejercitar",    Category = "verb", Accepts = "none"     },
        new Word { English = "invite",    Spanish = "invitar",      Category = "verb", Accepts = "person"   },
        new Word { English = "like",      Spanish = "gustar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "live",      Spanish = "vivir",        Category = "verb", Accepts = "place"    },
        new Word { English = "order",     Spanish = "ordenar/pedir",Category = "verb", Accepts = "food"    },
        new Word { English = "paint",     Spanish = "pintar",       Category = "verb", Accepts = "thing"    },
        new Word { English = "plan",      Spanish = "planear",      Category = "verb", Accepts = "abstract" },
        new Word { English = "rain",      Spanish = "llover",       Category = "verb", Accepts = "none"     },
        new Word { English = "shop",      Spanish = "comprar",      Category = "verb", Accepts = "thing"    },
        new Word { English = "smile",     Spanish = "sonreír",      Category = "verb", Accepts = "none"     },
        new Word { English = "snow",      Spanish = "nevar",        Category = "verb", Accepts = "none"     },
        new Word { English = "talk",      Spanish = "hablar",       Category = "verb", Accepts = "person"   },
        new Word { English = "wash",      Spanish = "lavar",        Category = "verb", Accepts = "thing"    },
    };

    static List<Complement> complements = new List<Complement>
    {
        new Complement { English = "tacos",         Spanish = "tacos",           Type = "food"     },
        new Complement { English = "food",          Spanish = "comida",          Type = "food"     },
        new Complement { English = "pizza",         Spanish = "pizza",           Type = "food"     },
        new Complement { English = "soup",          Spanish = "sopa",            Type = "food"     },
        new Complement { English = "a sandwich",    Spanish = "un sándwich",     Type = "food"     },
        new Complement { English = "rice",          Spanish = "arroz",           Type = "food"     },
        new Complement { English = "chicken",       Spanish = "pollo",           Type = "food"     },
        new Complement { English = "fish",          Spanish = "pescado",         Type = "food"     },
        new Complement { English = "eggs",          Spanish = "huevos",          Type = "food"     },
        new Complement { English = "bread",         Spanish = "pan",             Type = "food"     },
        new Complement { English = "cheese",        Spanish = "queso",           Type = "food"     },
        new Complement { English = "an apple",      Spanish = "una manzana",     Type = "food"     },
        new Complement { English = "a banana",      Spanish = "un plátano",      Type = "food"     },
        new Complement { English = "milk",          Spanish = "leche",           Type = "food"     },
        new Complement { English = "pasta",         Spanish = "pasta",           Type = "food"     },
        new Complement { English = "water",         Spanish = "agua",            Type = "drink"    },
        new Complement { English = "coffee",        Spanish = "café",            Type = "drink"    },
        new Complement { English = "juice",         Spanish = "jugo",            Type = "drink"    },
        new Complement { English = "tea",           Spanish = "té",              Type = "drink"    },
        new Complement { English = "cola",          Spanish = "refresco",        Type = "drink"    },
        new Complement { English = "beer",          Spanish = "cerveza",         Type = "drink"    },
        new Complement { English = "wine",          Spanish = "vino",            Type = "drink"    },
        new Complement { English = "the weapon",    Spanish = "el arma",         Type = "thing"    },
        new Complement { English = "the key",       Spanish = "la llave",        Type = "thing"    },
        new Complement { English = "the bag",       Spanish = "la bolsa",        Type = "thing"    },
        new Complement { English = "the sword",     Spanish = "la espada",       Type = "thing"    },
        new Complement { English = "the shield",    Spanish = "el escudo",       Type = "thing"    },
        new Complement { English = "the map",       Spanish = "el mapa",         Type = "thing"    },
        new Complement { English = "the car",       Spanish = "el carro",        Type = "thing"    },
        new Complement { English = "the phone",     Spanish = "el teléfono",     Type = "thing"    },
        new Complement { English = "the book",      Spanish = "el libro",        Type = "thing"    },
        new Complement { English = "the game",      Spanish = "el juego",        Type = "thing"    },
        new Complement { English = "the mission",   Spanish = "la misión",       Type = "thing"    },
        new Complement { English = "the door",      Spanish = "la puerta",       Type = "thing"    },
        new Complement { English = "the computer",  Spanish = "la computadora",  Type = "thing"    },
        new Complement { English = "the music",     Spanish = "la música",       Type = "thing"    },
        new Complement { English = "the house",     Spanish = "la casa",         Type = "thing"    },
        new Complement { English = "the letter",    Spanish = "la carta",        Type = "thing"    },
        new Complement { English = "the pen",       Spanish = "el bolígrafo",    Type = "thing"    },
        new Complement { English = "the pencil",    Spanish = "el lápiz",        Type = "thing"    },
        new Complement { English = "the paper",     Spanish = "el papel",        Type = "thing"    },
        new Complement { English = "the notebook",  Spanish = "la libreta",      Type = "thing"    },
        new Complement { English = "the desk",      Spanish = "el escritorio",   Type = "thing"    },
        new Complement { English = "the chair",     Spanish = "la silla",        Type = "thing"    },
        new Complement { English = "the table",     Spanish = "la mesa",         Type = "thing"    },
        new Complement { English = "the bed",       Spanish = "la cama",         Type = "thing"    },
        new Complement { English = "the bicycle",   Spanish = "la bicicleta",    Type = "thing"    },
        new Complement { English = "the camera",    Spanish = "la cámara",       Type = "thing"    },
        new Complement { English = "the watch",     Spanish = "el reloj",        Type = "thing"    },
        new Complement { English = "the shoes",     Spanish = "los zapatos",     Type = "thing"    },
        new Complement { English = "the shirt",     Spanish = "la camiseta",     Type = "thing"    },
        new Complement { English = "the pants",     Spanish = "los pantalones",  Type = "thing"    },
        new Complement { English = "my friend",     Spanish = "a mi amigo",      Type = "person"   },
        new Complement { English = "the enemy",     Spanish = "al enemigo",      Type = "person"   },
        new Complement { English = "my mom",        Spanish = "a mi mamá",       Type = "person"   },
        new Complement { English = "my dad",        Spanish = "a mi papá",       Type = "person"   },
        new Complement { English = "my sister",     Spanish = "a mi hermana",    Type = "person"   },
        new Complement { English = "my brother",    Spanish = "a mi hermano",    Type = "person"   },
        new Complement { English = "my cousin",     Spanish = "a mi primo/prima",Type = "person"   },
        new Complement { English = "the boss",      Spanish = "al jefe",         Type = "person"   },
        new Complement { English = "the soldier",   Spanish = "al soldado",      Type = "person"   },
        new Complement { English = "the student",   Spanish = "al estudiante",   Type = "person"   },
        new Complement { English = "the doctor",    Spanish = "al médico",       Type = "person"   },
        new Complement { English = "the teacher",   Spanish = "al maestro",      Type = "person"   },
        new Complement { English = "the nurse",     Spanish = "a la enfermera",  Type = "person"   },
        new Complement { English = "the driver",    Spanish = "al conductor",    Type = "person"   },
        new Complement { English = "the child",     Spanish = "al niño",         Type = "person"   },
        new Complement { English = "money",         Spanish = "dinero",          Type = "abstract" },
        new Complement { English = "time",          Spanish = "tiempo",          Type = "abstract" },
        new Complement { English = "help",          Spanish = "ayuda",           Type = "abstract" },
        new Complement { English = "energy",        Spanish = "energía",         Type = "abstract" },
        new Complement { English = "English",       Spanish = "inglés",          Type = "abstract" },
        new Complement { English = "math",          Spanish = "matemáticas",     Type = "abstract" },
        new Complement { English = "courage",       Spanish = "valor",           Type = "abstract" },
        new Complement { English = "patience",      Spanish = "paciencia",       Type = "abstract" },
        new Complement { English = "happiness",     Spanish = "felicidad",       Type = "abstract" },
        new Complement { English = "love",          Spanish = "amor",            Type = "abstract" },
        new Complement { English = "freedom",       Spanish = "libertad",        Type = "abstract" },
        new Complement { English = "knowledge",     Spanish = "conocimiento",    Type = "abstract" },
        new Complement { English = "a hamburger",   Spanish = "una hamburguesa", Type = "food"     },
        new Complement { English = "ice cream",     Spanish = "helado",          Type = "food"     },
        new Complement { English = "salad",         Spanish = "ensalada",        Type = "food"     },
        new Complement { English = "fruit",         Spanish = "fruta",           Type = "food"     },
        new Complement { English = "vegetables",    Spanish = "verduras",        Type = "food"     },
        new Complement { English = "cake",          Spanish = "pastel",          Type = "food"     },
        new Complement { English = "clothes",       Spanish = "ropa",            Type = "thing"    },
        new Complement { English = "a shirt",       Spanish = "una camisa",      Type = "thing"    },
        new Complement { English = "a gift",        Spanish = "un regalo",       Type = "thing"    },
        new Complement { English = "a ticket",      Spanish = "un boleto",       Type = "thing"    },
        new Complement { English = "a movie",       Spanish = "una película",    Type = "thing"    },
        new Complement { English = "homework",      Spanish = "tarea",           Type = "abstract" },
        new Complement { English = "a test",        Spanish = "un examen",       Type = "abstract" },
        new Complement { English = "music",         Spanish = "música",          Type = "abstract" },
        new Complement { English = "a story",       Spanish = "una historia",    Type = "abstract" },
        new Complement { English = "the weather",   Spanish = "el clima",        Type = "abstract" },
        new Complement { English = "a song",        Spanish = "una canción",     Type = "abstract" },
    };

    static List<Word> places = new List<Word>
    {
        new Word { English = "at home",              Spanish = "en casa"                  },
        new Word { English = "at the store",        Spanish = "en la tienda"             },
        new Word { English = "in the office",       Spanish = "en la oficina"            },
        new Word { English = "in the base",         Spanish = "en la base"               },
        new Word { English = "at school",           Spanish = "en la escuela"            },
        new Word { English = "in the forest",       Spanish = "en el bosque"             },
        new Word { English = "in the city",         Spanish = "en la ciudad"             },
        new Word { English = "by the river",        Spanish = "junto al río"             },
        new Word { English = "on the mountain",     Spanish = "en la montaña"            },
        new Word { English = "at the gym",          Spanish = "en el gimnasio"           },
        new Word { English = "in the castle",       Spanish = "en el castillo"           },
        new Word { English = "in the desert",       Spanish = "en el desierto"           },
        new Word { English = "at the park",         Spanish = "en el parque"             },
        new Word { English = "in the lab",          Spanish = "en el laboratorio"        },
        new Word { English = "at the hospital",     Spanish = "en el hospital"           },
        new Word { English = "in the cave",         Spanish = "en la cueva"              },
        new Word { English = "on the rooftop",      Spanish = "en la azotea"             },
        new Word { English = "underground",         Spanish = "bajo tierra"              },
        new Word { English = "at the restaurant",   Spanish = "en el restaurante"        },
        new Word { English = "in the kitchen",      Spanish = "en la cocina"             },
        new Word { English = "in the bedroom",      Spanish = "en la recámara"           },
        new Word { English = "in the bathroom",     Spanish = "en el baño"               },
        new Word { English = "in the living room",  Spanish = "en la sala"               },
        new Word { English = "at the beach",        Spanish = "en la playa"              },
        new Word { English = "at the library",      Spanish = "en la biblioteca"         },
        new Word { English = "in the market",       Spanish = "en el mercado"            },
        new Word { English = "in the church",       Spanish = "en la iglesia"            },
        new Word { English = "at the bus stop",     Spanish = "en la parada de autobús"  },
        new Word { English = "at the train station", Spanish = "en la estación de tren"   },
        new Word { English = "in the car",          Spanish = "en el carro"              },
        new Word { English = "on the bus",          Spanish = "en el autobús"            },
    };

    static List<Word> times = new List<Word>
    {
        new Word { English = "every day",           Spanish = "todos los días"           },
        new Word { English = "at night",            Spanish = "por las noches"           },
        new Word { English = "on Sundays",          Spanish = "los domingos"             },
        new Word { English = "on Monday",           Spanish = "el lunes"                 },
        new Word { English = "on Tuesday",          Spanish = "el martes"                },
        new Word { English = "on Wednesday",        Spanish = "el miércoles"             },
        new Word { English = "on Thursday",         Spanish = "el jueves"                },
        new Word { English = "on Friday",           Spanish = "el viernes"               },
        new Word { English = "on Saturday",         Spanish = "el sábado"                },
        new Word { English = "every morning",       Spanish = "cada mañana"              },
        new Word { English = "right now",           Spanish = "ahorita"                  },
        new Word { English = "every week",          Spanish = "cada semana"              },
        new Word { English = "in the afternoon",    Spanish = "por la tarde"             },
        new Word { English = "in the evening",      Spanish = "por la noche"             },
        new Word { English = "before work",         Spanish = "antes del trabajo"        },
        new Word { English = "after school",        Spanish = "después de la escuela"    },
        new Word { English = "on weekends",         Spanish = "los fines de semana"      },
        new Word { English = "last night",          Spanish = "anoche"                   },
        new Word { English = "yesterday",           Spanish = "ayer"                     },
        new Word { English = "last week",           Spanish = "la semana pasada"         },
        new Word { English = "last month",          Spanish = "el mes pasado"            },
        new Word { English = "last year",           Spanish = "el año pasado"            },
        new Word { English = "tomorrow",             Spanish = "mañana"                   },
        new Word { English = "next week",            Spanish = "la próxima semana"        },
        new Word { English = "next month",           Spanish = "el próximo mes"           },
        new Word { English = "in the morning",       Spanish = "por la mañana"            },
        new Word { English = "in the past",         Spanish = "en el pasado"             },
        new Word { English = "in the future",       Spanish = "en el futuro"             },
        new Word { English = "for hours",           Spanish = "por horas"                },
        new Word { English = "at 8 o'clock",        Spanish = "a las 8 en punto"         },
        new Word { English = "at noon",              Spanish = "al mediodía"              },
        new Word { English = "at midnight",         Spanish = "a la medianoche"          },
        new Word { English = "in the summer",       Spanish = "en el verano"             },
        new Word { English = "in the winter",       Spanish = "en el invierno"           },
        new Word { English = "in the spring",       Spanish = "en la primavera"          },
        new Word { English = "in the fall",         Spanish = "en el otoño"              },
    };

    static Random rnd = new Random();

    static readonly string[] thirdPersonSubjects = { "she", "he", "my mom", "snake", "mario", "link", "my friend", "the soldier", "the player" };

    static bool IsThirdPerson(string subject) =>
        thirdPersonSubjects.Contains(subject.ToLower());

    static string ConjugateEnglishPresent(string subject, string verb)
    {
        if (!IsThirdPerson(subject)) return verb;
        if (verb.EndsWith("y"))
        {
            char b = verb[verb.Length - 2];
            return "aeiou".Contains(b) ? verb + "s" : verb.Substring(0, verb.Length - 1) + "ies";
        }
        if (verb.EndsWith("s") || verb.EndsWith("sh") ||
            verb.EndsWith("ch") || verb.EndsWith("x") || verb.EndsWith("o"))
            return verb + "es";
        return verb + "s";
    }

    static string GetPast(string verb) =>
        verbList.FirstOrDefault(v => v.Infinitive == verb)?.Past ?? verb + "ed";

    static string GetParticiple(string verb) =>
        verbList.FirstOrDefault(v => v.Infinitive == verb)?.Participle ?? verb + "ed";

    static string GetGerund(string verb) =>
        verbList.FirstOrDefault(v => v.Infinitive == verb)?.Gerund ?? verb + "ing";

    static string GetSpanishInfinitive(string verb) =>
        verbList.FirstOrDefault(v => v.Infinitive == verb)?.SpanishInf ?? verb;

    static string GetSpanishGerund(string verb)
    {
        string inf = GetSpanishInfinitive(verb);
        if (inf.EndsWith("ar")) return inf.Substring(0, inf.Length - 2) + "ando";
        if (inf.EndsWith("er")) return inf.Substring(0, inf.Length - 2) + "iendo";
        if (inf.EndsWith("ir")) return inf.Substring(0, inf.Length - 2) + "iendo";
        
        if (inf == "ir") return "yendo";
        if (inf == "leer") return "leyendo";
        if (inf == "creer") return "creyendo";
        if (inf == "oír") return "oyendo";
        
        return inf;
    }

    static string GetSpanishPastParticiple(string verb)
    {
        string inf = GetSpanishInfinitive(verb);
        if (inf.EndsWith("ar")) return inf.Substring(0, inf.Length - 2) + "ado";
        if (inf.EndsWith("er") || inf.EndsWith("ir")) return inf.Substring(0, inf.Length - 2) + "ido";
        
        if (inf == "abrir") return "abierto";
        if (inf == "cubrir") return "cubierto";
        if (inf == "decir") return "dicho";
        if (inf == "escribir") return "escrito";
        if (inf == "hacer") return "hecho";
        if (inf == "morir") return "muerto";
        if (inf == "poner") return "puesto";
        if (inf == "romper") return "roto";
        if (inf == "ver") return "visto";
        if (inf == "volver") return "vuelto";
        
        return inf;
    }

    static string BuildConditional(string inf, bool isYo, bool isNos, Dictionary<string, string> stems)
    {
        string stem = inf;
        
        foreach (var kvp in stems)
        {
            if (inf.Contains(kvp.Key))
            {
                stem = inf.Replace(kvp.Key, kvp.Value);
                break;
            }
        }
        
        if (stem.EndsWith("ar"))
            return isYo ? stem.Replace("ar", "aría") : isNos ? stem.Replace("ar", "aríamos") : stem.Replace("ar", "aría");
        if (stem.EndsWith("er"))
            return isYo ? stem.Replace("er", "ería") : isNos ? stem.Replace("er", "eríamos") : stem.Replace("er", "ería");
        if (stem.EndsWith("ir"))
            return isYo ? stem.Replace("ir", "iría") : isNos ? stem.Replace("ir", "iríamos") : stem.Replace("ir", "iría");
        
        return isYo ? $"{inf}ía" : isNos ? $"{inf}íamos" : $"{inf}ía";
    }

    // ── Construir verbo inglés según tiempo (CORREGIDO) ──────────────────

    static string BuildEnglishVerb(string subject, string verb, Tense tense)
    {
        bool third = IsThirdPerson(subject);
        string subjLower = subject.ToLower();
        
        // CORREGIDO: manejo correcto de "am/are/is"
        string be;
        if (third)
            be = "is";
        else if (subjLower == "we" || subjLower == "they" || subjLower == "you")
            be = "are";
        else
            be = "am";
        
        // CORREGIDO: manejo correcto de "was/were"
        string was;
        if (third || subjLower == "i" || subjLower == "he" || subjLower == "she" || subjLower == "it")
            was = "was";
        else
            was = "were";

        return tense switch
        {
            Tense.PresentSimple     => ConjugateEnglishPresent(subject, verb),
            Tense.PresentContinuous => $"{be} {GetGerund(verb)}",
            Tense.PastSimple        => GetPast(verb),
            Tense.PastContinuous    => $"{was} {GetGerund(verb)}",
            Tense.FutureSimple      => $"will {verb}",
            Tense.PresentPerfect    => $"{(third ? "has" : "have")} {GetParticiple(verb)}",
            Tense.Conditional       => $"would {verb}",
            _ => verb
        };
    }

    // ── Construir verbo español según tiempo (CORREGIDO) ─────

    static string BuildSpanishVerb(string subject, string verb, Tense tense)
    {
		// Conjugación regular genérica para verbos no encontrados
            string inf = GetSpanishInfinitive(verb);
            string gerund = GetSpanishGerund(verb);
            string pastParticiple = GetSpanishPastParticiple(verb);
            bool isYo = subject.ToLower() == "i";
            bool isNos = subject.ToLower() == "we";
			
        if (!conjugaciones.TryGetValue(verb.ToLower(), out var c))
        {
            
            
            string regularPresent = isYo ? inf.Replace("ar", "o").Replace("er", "o").Replace("ir", "o")
                                 : isNos ? inf.Replace("ar", "amos").Replace("er", "emos").Replace("ir", "imos")
                                 : inf.Replace("ar", "a").Replace("er", "e").Replace("ir", "e");
            
            string regularPast = isYo ? inf.Replace("ar", "é").Replace("er", "í").Replace("ir", "í")
                               : isNos ? inf.Replace("ar", "amos").Replace("er", "imos").Replace("ir", "imos")
                               : inf.Replace("ar", "ó").Replace("er", "ió").Replace("ir", "ió");
            
            return tense switch
            {
                Tense.PresentSimple => regularPresent,
                Tense.PresentContinuous => isYo ? $"estoy {gerund}" : isNos ? $"estamos {gerund}" : $"está {gerund}",
                Tense.PastSimple => regularPast,
                Tense.PastContinuous => isYo ? $"estaba {gerund}" : isNos ? $"estábamos {gerund}" : $"estaba {gerund}",
                Tense.FutureSimple => isYo ? $"voy a {inf}" : isNos ? $"vamos a {inf}" : $"va a {inf}",
                Tense.PresentPerfect => isYo ? $"he {pastParticiple}" : isNos ? $"hemos {pastParticiple}" : $"ha {pastParticiple}",
                Tense.Conditional => isYo ? $"{inf.Replace("ar", "aría").Replace("er", "ería").Replace("ir", "iría")}" : 
                                      isNos ? $"{inf.Replace("ar", "aríamos").Replace("er", "eríamos").Replace("ir", "iríamos")}" :
                                      $"{inf.Replace("ar", "aría").Replace("er", "ería").Replace("ir", "iría")}",
                _ => regularPresent
            };
        }

        //bool  isYo = subject.ToLower() == "i";
        //bool isNos = subject.ToLower() == "we";
        string infinitive = GetSpanishInfinitive(verb);
        //string gerund = GetSpanishGerund(verb);
        //string pastParticiple = GetSpanishPastParticiple(verb);
        
        
        var conditionalStems = new Dictionary<string, string>
        {
            { "tener", "tendr" }, { "venir", "vendr" }, { "poder", "podr" },
            { "poner", "pondr" }, { "salir", "saldr" }, { "hacer", "har" },
            { "decir", "dir" },  { "querer", "querr" }, { "saber", "sabr" }
        };
        
        string future = isYo ? $"voy a {infinitive}" : isNos ? $"vamos a {infinitive}" : $"va a {infinitive}";

        return tense switch
        {
            Tense.PresentSimple => isYo ? c.YoPres : isNos ? c.NosPres : c.ElPres,
            Tense.PresentContinuous => isYo ? $"estoy {gerund}" : isNos ? $"estamos {gerund}" : $"está {gerund}",
            Tense.PastSimple => isYo ? c.YoPast : isNos ? c.NosPast : c.ElPast,
            Tense.PastContinuous => isYo ? $"estaba {gerund}" : isNos ? $"estábamos {gerund}" : $"estaba {gerund}",
            Tense.FutureSimple => future,
            Tense.PresentPerfect => isYo ? $"he {pastParticiple}" : isNos ? $"hemos {pastParticiple}" : $"ha {pastParticiple}",
            Tense.Conditional => BuildConditional(infinitive, isYo, isNos, conditionalStems),
            _ => c.ElPres
        };
    }

    // ── Generador de oraciones ───────────────────────────────

    enum SentenceMode { Normal, Question }

    static (string english, string spanish) GenerateSentence(
        bool includePlace  = true,
        bool includeTime   = true,
        SentenceMode mode  = SentenceMode.Normal,
        Tense? forcedTense = null)
    {
        var subj  = subjects[rnd.Next(subjects.Count)];
        var verb  = verbsForSentences[rnd.Next(verbsForSentences.Count)];
        var tense = forcedTense ?? (Tense)rnd.Next(7);

        string engVerb = BuildEnglishVerb(subj.English, verb.English, tense);
        string espVerb = BuildSpanishVerb(subj.English, verb.English, tense);

        string engComp = "", espComp = "";
        if (verb.Accepts != "none")
        {
            var compatible = complements.Where(c =>
                verb.Accepts == "any"
                || c.Type == verb.Accepts
                || (verb.Accepts == "abstract" &&
                    (c.Type == "abstract" || c.Type == "thing" || c.Type == "person"))
            ).ToList();

            if (compatible.Count > 0)
            {
                var comp = compatible[rnd.Next(compatible.Count)];
                engComp = $" {comp.English}";
                espComp = $" {comp.Spanish}";
            }
        }

        string engPlace = "", espPlace = "";
        if (includePlace && rnd.Next(2) == 0)
        {
            var p = places[rnd.Next(places.Count)];
            engPlace = $" {p.English}"; espPlace = $" {p.Spanish}";
        }

        string engTime = "", espTime = "";
        if (includeTime && rnd.Next(2) == 0)
        {
            var t = times[rnd.Next(times.Count)];
            engTime = $" {t.English}"; espTime = $" {t.Spanish}";
        }

        string eng, esp;
        string label = $"[{TenseName(tense)}]";

        if (mode == SentenceMode.Question)
        {
            bool third = IsThirdPerson(subj.English);
            bool isNos = subj.English.ToLower() == "we";

            string aux = tense switch
            {
                Tense.PresentSimple     => third ? "Does" : "Do",
                Tense.PresentContinuous => third ? "Is" : isNos ? "Are" : "Am",
                Tense.PastSimple        => "Did",
                Tense.PastContinuous    => third ? "Was" : "Were",
                Tense.FutureSimple      => "Will",
                Tense.PresentPerfect    => third ? "Has" : "Have",
                Tense.Conditional       => "Would",
                _ => "Do"
            };

            bool verbAlreadyFormed = tense == Tense.PresentContinuous
                                  || tense == Tense.PastContinuous
                                  || tense == Tense.PresentPerfect;

            string qVerb = verbAlreadyFormed
                ? engVerb.Substring(engVerb.IndexOf(' ') + 1)
                : verb.English;

            eng = $"{aux} {subj.English} {qVerb}{engComp}{engPlace}{engTime}?";
            esp = $"¿{subj.Spanish} {espVerb}{espComp}{espPlace}{espTime}?";
        }
        else
        {
            eng = $"{subj.English} {engVerb}{engComp}{engPlace}{engTime}.";
            esp = $"{subj.Spanish} {espVerb}{espComp}{espPlace}{espTime}.";
        }

        return ($"{label} {eng}", $"{label} {esp}");
    }

    // ── Menú principal ───────────────────────────────────────

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║    🎓 English Matrix v9 — Menú           ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║   ORACIONES                              ║");
            Console.WriteLine("║  1. Generar oraciones                    ║");
            Console.WriteLine("║  2. Generar preguntas                    ║");
            Console.WriteLine("║  3. Quiz — adivina la traducción         ║");
            Console.WriteLine("║  4. Quiz — modo pregunta                 ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║   VERBOS                                 ║");
            Console.WriteLine("║  5. Lista de verbos regulares            ║");
            Console.WriteLine("║  6. Lista de verbos irregulares          ║");
            Console.WriteLine("║  7. Quiz — verbos regulares              ║");
            Console.WriteLine("║  8. Quiz — verbos irregulares            ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║   TIEMPOS VERBALES                       ║");
            Console.WriteLine("║  9. Tabla de tiempos de un verbo         ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║   LECTURA                                ║");
            Console.WriteLine("║  10. Historias cortas en inglés          ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║  11. Agregar mis propias palabras        ║");
            Console.WriteLine("║  12. Ver todas mis palabras              ║");
            Console.WriteLine("║  13. Salir                               ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.Write("\nElige una opción: ");

            string option = Console.ReadLine();
            switch (option)
            {
                case "1":  ModeGenerate(SentenceMode.Normal);   break;
                case "2":  ModeGenerate(SentenceMode.Question); break;
                case "3":  ModeQuiz(SentenceMode.Normal);       break;
                case "4":  ModeQuiz(SentenceMode.Question);     break;
                case "5":  ModeVerbList(regular: true);         break;
                case "6":  ModeVerbList(regular: false);        break;
                case "7":  ModeVerbQuiz(regular: true);         break;
                case "8":  ModeVerbQuiz(regular: false);        break;
                case "9":  ModeTenseTable();                    break;
                case "10": ModeShortStories();                  break;
                case "11": ModeAddWords();                      break;
                case "12": ModeListWords();                     break;
                case "13": running = false;                     break;
                default:
                    Console.WriteLine("Opción no válida. Presiona Enter...");
                    Console.ReadLine(); break;
            }
        }
        Console.WriteLine("\n¡Hasta luego! Keep learning! 💪");
    }

    // ── Modo Generar ─────────────────────────────────────────

    static void ModeGenerate(SentenceMode mode)
    {
        Console.Clear();
        Console.WriteLine(mode == SentenceMode.Question
            ? "=== Generador de preguntas ===" : "=== Generador de oraciones ===");

        Console.WriteLine("\nTiempo verbal:");
        Console.WriteLine("  0. Aleatorio (mezcla todos)");
        Console.WriteLine("  1. Presente simple       (I eat)");
        Console.WriteLine("  2. Presente continuo     (I am eating)");
        Console.WriteLine("  3. Pasado simple         (I ate)");
        Console.WriteLine("  4. Pasado continuo       (I was eating)");
        Console.WriteLine("  5. Futuro simple         (I will eat)");
        Console.WriteLine("  6. Presente perfecto     (I have eaten)");
        Console.WriteLine("  7. Condicional           (I would eat)");
        Console.Write("Elige (0-7): ");

        Tense? tense = Console.ReadLine() switch
        {
            "1" => Tense.PresentSimple,
            "2" => Tense.PresentContinuous,
            "3" => Tense.PastSimple,
            "4" => Tense.PastContinuous,
            "5" => Tense.FutureSimple,
            "6" => Tense.PresentPerfect,
            "7" => Tense.Conditional,
            _   => null
        };

        Console.Write("\n¿Cuántas quieres generar? (1-20): ");
        if (!int.TryParse(Console.ReadLine(), out int count) || count < 1 || count > 20)
            count = 5;

        Console.WriteLine();
        for (int i = 1; i <= count; i++)
        {
            var (eng, esp) = GenerateSentence(mode: mode, forcedTense: tense);
            Console.WriteLine($"  {i}. EN: {eng}");
            Console.WriteLine($"     ES: {esp}");
            Console.WriteLine();
        }

        Console.WriteLine("Presiona Enter para volver al menú...");
        Console.ReadLine();
    }

    // ── Quiz oraciones ───────────────────────────────────────

    static void ModeQuiz(SentenceMode mode)
    {
        Console.Clear();
        Console.WriteLine(mode == SentenceMode.Question
            ? "=== Quiz — Preguntas ===" : "=== Quiz — Oraciones ===");
        Console.WriteLine("Escribe 'salir' para terminar.\n");

        int correct = 0, total = 0;
        while (true)
        {
            var (eng, esp) = GenerateSentence(mode: mode);
            Console.WriteLine($"EN: {eng}");
            Console.Write("Tu traducción: ");
            string answer = Console.ReadLine();
            if (answer?.ToLower() == "salir") break;
            total++;
            Console.WriteLine($"Respuesta: {esp}");
            Console.Write("¿La tuviste bien? (s/n): ");
            if (Console.ReadLine()?.ToLower() == "s") correct++;
            Console.WriteLine();
        }

        if (total > 0)
        {
            int pct = (correct * 100) / total;
            Console.WriteLine($"\nResultado: {correct}/{total} ({pct}%)");
            if      (pct >= 80) Console.WriteLine("¡Excelente! 🏆");
            else if (pct >= 50) Console.WriteLine("¡Bien! Sigue practicando 💪");
            else                Console.WriteLine("¡No te rindas! Keep going! 🎮");
        }

        Console.WriteLine("\nPresiona Enter...");
        Console.ReadLine();
    }

    // ── Lista de verbos ──────────────────────────────────────

    static void ModeVerbList(bool regular)
    {
        Console.Clear();
        var list = verbList.Where(v => v.IsRegular == regular).OrderBy(v => v.Infinitive).ToList();
        string title = regular ? $"=== Verbos Regulares ({list.Count}) ===" : $"=== Verbos Irregulares ({list.Count}) ===";
        Console.WriteLine(title);
        Console.WriteLine($"\n  {"Infinitivo",-16} {"Pasado",-16} {"Participio",-16} {"Gerundio",-16} {"Español",-18}");
        Console.WriteLine(new string('─', 84));

        foreach (var v in list)
            Console.WriteLine($"  {v.Infinitive,-16} {v.Past,-16} {v.Participle,-16} {v.Gerund,-16} {v.SpanishInf,-18}");

        Console.WriteLine("\nPresiona Enter...");
        Console.ReadLine();
    }

    // ── Quiz de verbos ────────────────────────────────────────

    static void ModeVerbQuiz(bool regular)
    {
        Console.Clear();
        Console.WriteLine(regular ? "=== Quiz — Verbos Regulares ===" : "=== Quiz — Verbos Irregulares ===");
        Console.WriteLine("Se muestra el infinitivo. Escribe pasado, participio y gerundio.");
        Console.WriteLine("Escribe 'salir' para terminar.\n");

        var pool = verbList.Where(v => v.IsRegular == regular).ToList();
        int correct = 0, total = 0;

        while (true)
        {
            var v = pool[rnd.Next(pool.Count)];
            Console.WriteLine($"Verbo: {v.Infinitive} ({v.SpanishInf})");

            Console.Write("  Pasado     (past):        "); string past = Console.ReadLine()?.Trim().ToLower();
            if (past == "salir") break;
            Console.Write("  Participio (participle):  "); string part = Console.ReadLine()?.Trim().ToLower();
            if (part == "salir") break;
            Console.Write("  Gerundio   (gerund -ing): "); string ger  = Console.ReadLine()?.Trim().ToLower();
            if (ger == "salir") break;

            total++;
            bool pastOk = past == v.Past.ToLower();
            bool partOk = part == v.Participle.ToLower();
            bool gerOk  = ger  == v.Gerund.ToLower();

            if (pastOk && partOk && gerOk)
            {
                correct++;
                Console.WriteLine("  ✅ ¡Perfecto!\n");
            }
            else
            {
                if (!pastOk) Console.WriteLine($"  ✗ Pasado:     correcto = {v.Past}");
                if (!partOk) Console.WriteLine($"  ✗ Participio: correcto = {v.Participle}");
                if (!gerOk)  Console.WriteLine($"  ✗ Gerundio:   correcto = {v.Gerund}");
                Console.WriteLine();
            }
        }

        if (total > 0)
        {
            int pct = (correct * 100) / total;
            Console.WriteLine($"\nResultado: {correct}/{total} ({pct}%)");
            if      (pct >= 80) Console.WriteLine("¡Excelente! 🏆");
            else if (pct >= 50) Console.WriteLine("¡Bien! Sigue practicando 💪");
            else                Console.WriteLine("¡No te rindas! Keep going! 🎮");
        }

        Console.WriteLine("\nPresiona Enter...");
        Console.ReadLine();
    }

    // ── Tabla de tiempos ─────────────────────────────────────

    static void ModeTenseTable()
    {
        Console.Clear();
        Console.WriteLine("=== Tabla de tiempos de un verbo ===\n");
        Console.Write("Escribe el verbo en inglés (ej: eat, run, work): ");
        string input = Console.ReadLine()?.Trim().ToLower();

        if (!conjugaciones.ContainsKey(input))
        {
            Console.WriteLine($"\nNo tengo el verbo '{input}' en el diccionario.");
            Console.WriteLine("Presiona Enter...");
            Console.ReadLine(); return;
        }

        Console.WriteLine($"\n  Verbo: {input} — {GetSpanishInfinitive(input)}\n");
        Console.WriteLine($"  {"Tiempo",-22} {"I",-28} {"She/He",-28} {"We",-28}");
        Console.WriteLine(new string('─', 110));

        foreach (Tense t in Enum.GetValues(typeof(Tense)))
        {
            string engI   = BuildEnglishVerb("I",   input, t);
            string engShe = BuildEnglishVerb("She", input, t);
            string engWe  = BuildEnglishVerb("We",  input, t);
            Console.WriteLine($"  {TenseName(t),-22} {"I " + engI,-28} {"She " + engShe,-28} {"We " + engWe,-28}");
        }

        Console.WriteLine($"\n  {"Tiempo",-22} {"Yo",-28} {"Él/Ella",-28} {"Nosotros",-28}");
        Console.WriteLine(new string('─', 110));

        foreach (Tense t in Enum.GetValues(typeof(Tense)))
        {
            string espI   = BuildSpanishVerb("I",   input, t);
            string espShe = BuildSpanishVerb("She", input, t);
            string espWe  = BuildSpanishVerb("We",  input, t);
            Console.WriteLine($"  {TenseName(t),-22} {"Yo " + espI,-28} {"Ella " + espShe,-28} {"Nos. " + espWe,-28}");
        }

        Console.WriteLine("\nPresiona Enter...");
        Console.ReadLine();
    }

    // ── Historias cortas (Mantén tu implementación original) ──
    // Nota: Por brevedad, aquí van solo las historias más importantes
    // Reemplaza esta sección con tus historias originales completas

    class ShortStory
    {
        public int    Id    { get; set; }
        public string Level { get; set; }
        public string Title { get; set; }
        public string Body  { get; set; }
    }

    static List<ShortStory> stories = new List<ShortStory>
    {
        new ShortStory { Id=1,  Level="A2", Title="Daniel el doctor",
            Body =
@"Hello! My name is Daniel. I am 38 years old, and I am from Colombia.
In my country, I worked as a doctor in a very famous clinic.
I loved my job because I helped many people every day.
Now, I live in the United States. My life is very different here. I work in construction.
My salary is very good, but I work very long hours.
I start work at 6:00 a.m. and finish at 7:00 p.m. every day.
Sometimes I feel tired because the work is difficult.
I miss my old job and my country, but I continue working hard for a better future." },

        new ShortStory { Id=2,  Level="A1", Title="Julieta aprende inglés",
            Body =
@"Hello! My name is Julieta, and I am 80 years old.
I live with my husband in a small house.
I like learning new things. Now, I am learning English at home.
I read books, watch movies, and listen to music in English every day.
Sometimes English is difficult, but I enjoy practicing new words and simple sentences.
I study a little every morning and evening.
Learning English makes me feel happy because I can understand more things
and communicate with new people.
I believe it is never too late to learn something new." },

        new ShortStory { Id=3,  Level="A1", Title="Los lunes",
            Body =
@"I don't like Mondays because I have to get up early to make breakfast for my children
and sometimes I'm late for work.
I'm busy all day and I come home very tired.
Monday is the worst day of the week." },

        new ShortStory { Id=4,  Level="C1", Title="Valeria: Speak with Power",
            Body =
@"Last year, Valeria avoided public speaking due to an intense fear of making mistakes.
Despite her strong knowledge, she often declined important presentations,
which limited her professional growth.
Recently, she has been confronting her fear through constant practice and feedback.
Next month, she will deliver a keynote speech at an international conference.
She now believes that embracing discomfort is the key to unlocking new opportunities." },

        new ShortStory { Id=5,  Level="A1", Title="Alice, estudiante de medicina",
            Body =
@"Alice is a university student. She studies medicine.
Alice lives with her parents in a small apartment in Madrid.
She enjoys listening to pop music and watching series.
Alice is a good cook, too." },

        new ShortStory { Id=6,  Level="A1", Title="Emily, mi mamá",
            Body =
@"Emily is my mother. She is 32 years old.
She works at an excellent school in the city.
She is a good teacher and a very responsible woman." },

        new ShortStory { Id=7,  Level="A1", Title="La lista del súper",
            Body =
@"I'm 45 years old. When I go to the supermarket,
I always write a list to buy all the vegetables and fruits
because my memory is very poor,
but many times I forget the list at home or don't know where it is." },

        new ShortStory { Id=8,  Level="A1", Title="Lily y su familia",
            Body =
@"Hello! My name is Lily and I am 43 years old.
I work at a company eight hours a day, from Monday to Saturday.
I don't have much time for my husband and my four children." },

        new ShortStory { Id=9,  Level="A1", Title="Edward y los animales",
            Body =
@"My name is Edward, and I'm fourteen years old.
I like adopting abandoned animals because I have a big heart.
At home, there are three dogs and two female cats." },

        new ShortStory { Id=10, Level="A1", Title="Grace va a la escuela",
            Body =
@"My name is Grace. I am seven years old. Today I start second grade.
I have new pencils and books.
My grandma made me pancakes and fruit salad for my lunch.
I am ready for school!" },

        new ShortStory { Id=11, Level="A1", Title="Mercedes aprende",
            Body =
@"My name is Mercedes and I am a happy grandmother.
I am 68 years old and I want to speak English well.
I copy lessons in my notebook every morning.
I read my notes before going to sleep." },

        new ShortStory { Id=12, Level="A1", Title="La vida adulta",
            Body =
@"I work every day, but I do not have money for me.
I pay rent, electricity, water, and buy fruits and vegetables.
I do not want to be an adult." },

        new ShortStory { Id=13, Level="A1", Title="Alex, papá tecnológico",
            Body =
@"Hello everyone! This is my dad, Alex.
He is smart and good with technology. He didn't go to university,
but he can fix computers and phones. He wanted to be an engineer." },

        new ShortStory { Id=14, Level="A1", Title="Recuerdos con mamá",
            Body =
@"I have very special memories with my mother. She was my best friend.
Our favorite activity was cooking together.
She was a very good cook, and now I have a restaurant." },

        new ShortStory { Id=15, Level="A1", Title="Lejos de casa",
            Body =
@"I live alone in Canada for work. My family lives in Mexico, my native country.
During my day, I have many solitary moments.
Sometimes I cry because I miss my family very much." },

        new ShortStory { Id=16, Level="A1", Title="Un buen papá",
            Body =
@"When I was a child, my dad was very busy. He did not have time to play with me.
Now I am a father. I want to be a good dad.
I want to show my son he is important to me." },

        new ShortStory { Id=17, Level="A1", Title="Aprender con películas",
            Body =
@"I am a young woman, and I like to learn languages.
Now I am learning English. My method is to watch English movies for kids
and read interesting stories.
I do this every day because I know the brain needs repetition." },

        new ShortStory { Id=18, Level="A1", Title="Abby no quiere ir a la escuela",
            Body =
@"Hello, my name is Abby. I do not want to go to school.
I do not have any friends in my class.
The other children do not want to play with me.
I want to change schools." },

        new ShortStory { Id=19, Level="A1", Title="Mi día favorito",
            Body =
@"My favorite day of the week is Saturday.
I don't have to go to school, and I can wake up late.
In the afternoon, I go to the park to play with my dog, Thor.
In the evening, I cook with my mom." },

        new ShortStory { Id=20, Level="A1", Title="Samuel aprende inglés",
            Body =
@"My name is Samuel, a happy grandfather. I am 70 years old, and I want to learn English.
I study vocabulary and reading every day.
I like to read simple stories. I love learning English." },

        new ShortStory { Id=21, Level="A1", Title="Lily canta en la ducha",
            Body =
@"Hi everyone! My name is Lily. Every morning, I take a shower to get ready for work.
I like to play my favorite music and sing.
My mom says I sing badly, but I just enjoy the moment." },

        new ShortStory { Id=22, Level="A1", Title="Necesito vacaciones",
            Body =
@"I'm stressed. I need a vacation.
I want to go to the beach with my friends to play volleyball, sunbathe, and relax.
The beach is very beautiful. But the problem is I don't have much money." },

        new ShortStory { Id=23, Level="A1", Title="Robert, el chef sin universidad",
            Body =
@"My father's name is Robert. He is a very talented man.
He didn't go to university, but he can cook Mexican, Japanese, Chinese,
and Peruvian food very well. He wanted to be a professional chef." },

        new ShortStory { Id=24, Level="A1", Title="Ana y su papá doctor",
            Body =
@"My name is Ana and I am nine years old. My dad's name is Carlos.
He works all day at a hospital.
I want to play with him, but he is almost never at home.
His job is very important." },

        new ShortStory { Id=25, Level="A1", Title="Un papá estudiante",
            Body =
@"My father is 25 years old now.
He left university to take care of me when I was a baby.
Now he's studying again and he's a good student.
He will be an engineer soon." },

        new ShortStory { Id=26, Level="A1", Title="Paul y su bebé",
            Body =
@"My name is Paul and I am 38 years old.
I live in a small house with my wife and my baby.
I work at a bank, five days a week.
Every morning, we read books to our baby. We are a happy family." },

        new ShortStory { Id=27, Level="A1", Title="Carla y sus rutinas",
            Body =
@"My name is Carla. My sister calls me ""Carlita"". I work at a school during the day.
On weekends, I go to the park.
But first, I clean my room and help my dad wash the car." },

        new ShortStory { Id=28, Level="A1", Title="Maria, enfermera retirada",
            Body =
@"My name is Maria. I'm 68 years old. I was a nurse, but now I'm retired.
I live in a small house with my dog.
Sometimes I feel sad because my family doesn't visit me." },

        new ShortStory { Id=29, Level="A1", Title="Richard, primera entrevista",
            Body =
@"My name is Richard, and I'm an architect.
Today is a special day because I have my first job interview at an important company.
I'm nervous, but very happy." },

        new ShortStory { Id=30, Level="A1", Title="Visita a la abuela",
            Body =
@"Sundays are special because we visit my grandma.
We prepare delicious food and have hot coffee.
She lives alone in the countryside because she doesn't like the noise of the city.
She prefers to be peacefully at home with her pet." },

        new ShortStory { Id=31, Level="A1", Title="Sofia en el parque",
            Body =
@"Hello, I'm Sofia. I like going for a run every morning.
I always see a guy in the park. He has black hair and green eyes.
I want to talk to him, but I'm shy." },

        new ShortStory { Id=32, Level="A1", Title="Ana, madre soltera",
            Body =
@"Hi, my name is Ana. I am a single mother.
In the morning, I take care of my daughter.
In the afternoon I work, and at night I study online.
Sometimes I am very tired, but I want a better future for us." },

        new ShortStory { Id=33, Level="A1", Title="Miguel y el carro",
            Body =
@"My name is Miguel, and I'm a doctor.
I always go to work by car, but today is a bad day
because my car has a problem.
I have two options: take it to the mechanic or take the bus." },

        new ShortStory { Id=34, Level="A1", Title="El sábado perfecto",
            Body =
@"Saturday is my favorite day because I start with a yoga class.
Afterward, I have lunch somewhere in the city with my best friend.
In the evening, I like to read a good book to end my day without stress." },

        new ShortStory { Id=35, Level="A1", Title="Sara en Barcelona",
            Body =
@"My name is Sara, and I'm 43 years old. I live in Barcelona with my family.
I have two sons and one daughter.
I work at a coffee shop five days a week.
On Saturdays we visit my parents." },

        new ShortStory { Id=36, Level="A1", Title="Daisy y sus clases",
            Body =
@"My name is Daisy. I am 65 years old, and I want to speak English.
I have online English lessons and study at home every day.
The teacher is very patient with me. She is an excellent teacher." },

        new ShortStory { Id=37, Level="A1", Title="Alex en Australia",
            Body =
@"My name is Alex and I live in Australia.
I'm busy all day because I work in the morning and study at night.
I'm a waiter in a restaurant and, honestly, I don't enjoy life." },

        new ShortStory { Id=38, Level="A1", Title="Maestra en línea",
            Body =
@"I work from home because I'm an online teacher. I teach English online.
My husband works in an office. He's an economist.
We rent an apartment. We work every day to buy a house and a car." },

        new ShortStory { Id=39, Level="A1", Title="El doctor y el ejercicio",
            Body =
@"The doctor recommends that I exercise every day for 30 min and drink water.
This is hard for me because I don't like going to the gym.
I like drinking soda, eating bread and chocolate cake." },

        new ShortStory { Id=40, Level="A1", Title="Sam y Lucy",
            Body =
@"My brother Sam is a famous doctor. He's a busy man.
Sam always works at the hospital with his wife Lucy.
She's a responsible nurse. Lucy and Sam don't have any children yet." },

        new ShortStory { Id=41, Level="A1", Title="Lisa y la cocina",
            Body =
@"My name is Lisa, and I have two kids, but I am not good at cooking.
My kids want to eat at a restaurant, but I do not have much money.
I want to cook like my mom; she cooks very well." },

        new ShortStory { Id=42, Level="A1", Title="Eva, niña feliz",
            Body =
@"My name is Eva and I'm seven years old.
I like to eat apples and drink milk. My favorite color is pink.
I play with my four friends in the park every day.
I'm a very happy girl." },

        new ShortStory { Id=43, Level="A1", Title="Andrea en New York",
            Body =
@"My name is Andrea, and I'm 29 years old.
I live in a white house in New York with my mom.
We have three cats and one dog.
I'm a teacher and work at a school from Monday to Friday." },

        new ShortStory { Id=44, Level="A1", Title="Charlotte y la medicina",
            Body =
@"I'm Charlotte. My favorite activity is cooking.
I'm not happy because my mother selected my university career.
She's an excellent doctor, and I'm studying medicine now.
But I don't want to be a doctor." },
    };

    static void ModeShortStories()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║   📖 Historias cortas en inglés          ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║  1. Leer una historia aleatoria          ║");
            Console.WriteLine("║  2. Elegir historia por número           ║");
            Console.WriteLine("║  3. Filtrar por nivel (A1 / A2 / C1)     ║");
            Console.WriteLine("║  4. Ver lista de todas las historias     ║");
            Console.WriteLine("║  0. Volver al menú principal             ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");
            Console.Write("\nElige: ");
            string opt = Console.ReadLine();

            switch (opt)
            {
                case "1":
                    PrintStory(stories[rnd.Next(stories.Count)]);
                    break;
                case "2":
                    Console.Write($"Número de historia (1-{stories.Count}): ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var s = stories.FirstOrDefault(x => x.Id == id);
                        if (s != null) PrintStory(s);
                        else { Console.WriteLine("Historia no encontrada. Presiona Enter..."); Console.ReadLine(); }
                    }
                    break;
                case "3":
                    Console.Write("Nivel (A1 / A2 / C1): ");
                    string lvl = Console.ReadLine()?.Trim().ToUpper() ?? "";
                    var filtered = stories.Where(x => x.Level == lvl).ToList();
                    if (filtered.Count == 0)
                    {
                        Console.WriteLine("No hay historias para ese nivel. Presiona Enter...");
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"=== Historias nivel {lvl} ===\n");
                        foreach (var st in filtered)
                            Console.WriteLine($"  [{st.Id:D2}] {st.Title}");
                        Console.Write($"\nElige número (1-{stories.Count}) o Enter para volver: ");
                        string pick = Console.ReadLine();
                        if (int.TryParse(pick, out int pid))
                        {
                            var chosen = filtered.FirstOrDefault(x => x.Id == pid);
                            if (chosen != null) PrintStory(chosen);
                        }
                    }
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("=== Todas las historias ===\n");
                    foreach (var st in stories)
                        Console.WriteLine($"  [{st.Id:D2}] ({st.Level}) {st.Title}");
                    Console.WriteLine("\nPresiona Enter...");
                    Console.ReadLine();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Presiona Enter...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void PrintStory(ShortStory s)
    {
        Console.Clear();
        Console.WriteLine($"╔══════════════════════════════════════════╗");
        Console.WriteLine($"║  Historia #{s.Id:D2}  —  Nivel: {s.Level,-17}║");
        Console.WriteLine($"║  {s.Title,-40}║");
        Console.WriteLine($"╚══════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine(s.Body);
        Console.WriteLine();
        Console.WriteLine("Presiona Enter para continuar...");
        Console.ReadLine();
    }

    // ── Agregar palabras ──────────────────────────────────────

    static void ModeAddWords()
    {
        Console.Clear();
        Console.WriteLine("=== Agregar mis propias palabras ===\n");
        Console.WriteLine("  1. Sujeto   2. Verbo   3. Complemento   4. Lugar   5. Tiempo");
        Console.Write("\nElige (1-5): ");

        string category = Console.ReadLine() switch
        {
            "1" => "subject", "2" => "verb", "3" => "complement",
            "4" => "place",   "5" => "time",  _ => ""
        };

        if (string.IsNullOrEmpty(category))
        {
            Console.WriteLine("Inválido. Presiona Enter...");
            Console.ReadLine(); return;
        }

        Console.Write("Palabra en inglés: ");
        string eng = Console.ReadLine();
        Console.Write("Traducción en español: ");
        string esp = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(eng) || string.IsNullOrWhiteSpace(esp))
        {
            Console.WriteLine("Campos vacíos. Presiona Enter...");
            Console.ReadLine(); return;
        }

        if (category == "verb")
        {
            Console.Write("Pasado:      "); string past = Console.ReadLine();
            Console.Write("Participio:  "); string part = Console.ReadLine();
            Console.Write("Gerundio:    "); string ger  = Console.ReadLine();
            Console.Write("¿Regular? (s/n): "); bool isReg = Console.ReadLine()?.ToLower() == "s";

            verbList.Add(new VerbEntry { Infinitive=eng, Past=past, Participle=part, Gerund=ger, SpanishInf=esp, IsRegular=isReg });

            Console.WriteLine($"\nConjugaciones de '{esp}':");
            Console.Write("  YO presente:        "); string yoPr  = Console.ReadLine();
            Console.Write("  ÉL/ELLA presente:   "); string elPr  = Console.ReadLine();
            Console.Write("  NOSOTROS presente:  "); string nosPr = Console.ReadLine();
            Console.Write("  YO pasado:          "); string yoPa  = Console.ReadLine();
            Console.Write("  ÉL/ELLA pasado:     "); string elPa  = Console.ReadLine();
            Console.Write("  NOSOTROS pasado:    "); string nosPa = Console.ReadLine();

            conjugaciones[eng.ToLower()] = new SpanishConj
                { YoPres=yoPr, ElPres=elPr, NosPres=nosPr, YoPast=yoPa, ElPast=elPa, NosPast=nosPa };

            Console.WriteLine("\n¿Complemento que acepta?");
            Console.WriteLine("  1.Comida 2.Bebida 3.Objeto 4.Persona 5.Abstracto 6.Ninguno");
            Console.Write("Elige (1-6): ");
            string accepts = Console.ReadLine() switch
            {
                "1"=>"food","2"=>"drink","3"=>"thing",
                "4"=>"person","5"=>"abstract","6"=>"none",_=>"abstract"
            };
            verbsForSentences.Add(new Word { English=eng, Spanish=esp, Category="verb", Accepts=accepts });
        }
        else if (category == "complement")
        {
            Console.WriteLine("Tipo: 1.Comida 2.Bebida 3.Objeto 4.Persona 5.Abstracto");
            Console.Write("Elige (1-5): ");
            string type = Console.ReadLine() switch
            {
                "1"=>"food","2"=>"drink","3"=>"thing","4"=>"person","5"=>"abstract",_=>"thing"
            };
            complements.Add(new Complement { English=eng, Spanish=esp, Type=type });
        }
        else subjects.Add(new Word { English=eng, Spanish=esp, Category=category });

        Console.WriteLine($"\n✅ '{eng}' agregado. Presiona Enter...");
        Console.ReadLine();
    }

    // ── Ver todas las palabras ────────────────────────────────

    static void ModeListWords()
    {
        Console.Clear();
        Console.WriteLine("=== Todas mis palabras ===\n");

        Console.WriteLine("── Sujetos ──");
        foreach (var w in subjects)
            Console.WriteLine($"  {w.English,-22} = {w.Spanish}");

        Console.WriteLine($"\n── Verbos para oraciones ({verbsForSentences.Count}) ──");
        foreach (var v in verbsForSentences)
            Console.WriteLine($"  {v.English,-14} acepta: {v.Accepts}");

        Console.WriteLine($"\n── Verbos en lista ({verbList.Count} total: {verbList.Count(v=>v.IsRegular)} reg / {verbList.Count(v=>!v.IsRegular)} irreg) ──");

        Console.WriteLine("\n── Complementos ──");
        foreach (var c in complements)
            Console.WriteLine($"  {c.English,-22} = {c.Spanish,-25} [{c.Type}]");

        Console.WriteLine("\n── Lugares ──");
        foreach (var w in places)
            Console.WriteLine($"  {w.English,-28} = {w.Spanish}");

        Console.WriteLine("\n── Tiempos ──");
        foreach (var w in times)
            Console.WriteLine($"  {w.English,-28} = {w.Spanish}");

        Console.WriteLine("\nPresiona Enter...");
        Console.ReadLine();
    }
}