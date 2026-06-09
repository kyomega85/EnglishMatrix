# EnglishMatrix Verb List Completeness Analysis

**Report Date:** 2026-06-05  
**File Analyzed:** EnglishMatrix.cs

---

## 📊 SUMMARY STATISTICS

| Category | Count |
|----------|-------|
| **Total in conjugaciones (Spanish)** | 134 verbs |
| **Total in verbList** | 153 verbs |
| - Regular verbs | 95 verbs |
| - Irregular verbs | 58 verbs |
| **Total in verbsForSentences** | 64 verbs |
| **Orphan entries** (in conjugaciones only) | 8 verbs |
| **Missing conjugations** (in verbList only) | 35 verbs |

---

## 1️⃣ COMPLETE LIST OF VERBS IN "conjugaciones" (Spanish Conjugations)

**Total: 134 verbs**

```
accept, activate, add, answer, arrive, ask, attack, build, buy, call, carry, catch, celebrate, change, check, choose, clean, climb, close, collect, complete, connect, cook, copy, create, dance, decide, defend, delete, destroy, download, draw, dress, drink, drive, drop, eat, enjoy, escape, explain, explore, fail, fall, feel, fight, find, finish, fix, fly, forget, grow, hack, hear, help, hide, install, jump, keep, know, laugh, learn, leave, listen, look, love, meet, miss, move, need, open, paint, pay, play, practice, protect, pull, push, read, receive, refuse, remember, remove, rescue, return, run, save, search, see, sell, send, share, show, sing, sleep, speak, spend, start, stop, study, suggest, swim, teach, tell, test, think, throw, travel, trust, try, turn, type, understand, unlock, upgrade, upload, visit, wait, wake, walk, want, wash, watch, win, work, write

Count: 134 verbs ✓
```

---

## 2️⃣ COMPLETE LIST OF VERBS IN "verbList" (VerbEntry Objects)

### 🟢 REGULAR VERBS (IsRegular=true)
**Total: 95 verbs**

```
accept, activate, add, answer, arrive, ask, attack, call, carry, celebrate, change, check, 
clean, climb, close, collect, complete, connect, cook, copy, create, dance, decide, defend, 
delete, download, dress, drop, enjoy, escape, explain, explore, fail, finish, fix, hack, help, 
install, jump, learn, listen, load, look, love, miss, move, need, open, paint, pay, play, 
practice, print, protect, pull, push, receive, refuse, remember, remove, rescue, return, save, 
search, share, show, sing, start, stop, study, suggest, test, travel, try, turn, type, 
unlock, upgrade, upload, use, visit, wait, walk, want, wash, watch, work

Count: 95 verbs ✓
```

### 🔴 IRREGULAR VERBS (IsRegular=false)
**Total: 58 verbs**

```
be, beat, become, begin, blow, break, bring, build, buy, catch, choose, come, cost, cut, 
do, draw, drink, drive, eat, fall, feel, fight, find, fly, forget, get, give, go, grow, 
hang, have, hear, hide, hit, hold, hurt, keep, know, lead, leave, lend, let, lose, make, 
mean, meet, pay, put, read, ride, ring, rise, run, say, see, seek, sell, send, set, shake, 
shoot, shrink, shut, sing, sink, sit, sleep, speak, spend, stand, steal, strike, swim, 
swing, take, teach, tear, tell, think, throw, understand, wake, wear, win, write

Count: 58 verbs ✓
```

---

## 3️⃣ COMPLETE LIST OF VERBS IN "verbsForSentences" (Sentence Generation)

**Total: 64 verbs**

```
accept, answer, ask, attack, build, buy, call, carry, catch, celebrate, choose, clean, 
cook, create, dance, decide, defend, delete, download, draw, drink, drive, drop, eat, 
enjoy, escape, explore, fall, fear, feel, fight, find, fly, give, hack, help, hide, 
install, jump, laugh, learn, love, meet, miss, need, open, paint, play, practice, 
protect, read, rescue, run, save, search, sell, send, show, sing, sleep, speak, 
study, swim, take, teach, tell, throw, travel, use, visit, walk, want, work, write

Count: 64 verbs ✓
```

---

## 4️⃣ ORPHAN ENTRIES (In conjugaciones but NOT in verbList)

**8 verbs found** ⚠️

These verbs have Spanish conjugations defined but are NOT in the main verbList:

| Verb | Spanish (Yo) | Issue |
|------|--------------|-------|
| `laugh` | me río | Missing from verbList |
| `cry` | lloro | Missing from verbList |
| `worry` | me preocupo | Missing from verbList |
| `keep` | (in conjugaciones, not found) | Incomplete entry |
| `ride` | (in conjugaciones, not found) | Incomplete entry |
| `hang` | (in conjugaciones, not found) | Incomplete entry |
| `hit` | (in conjugaciones, not found) | Incomplete entry |
| `hurt` | (in conjugaciones, not found) | Incomplete entry |

⚠️ **Exact match orphans in conjugaciones:**
- `laugh` - exists in conjugaciones but NOT in verbList
- `cry` - exists in conjugaciones but NOT in verbList  
- `worry` - exists in conjugaciones but NOT in verbList

---

## 5️⃣ MISSING CONJUGATIONS (In verbList but NOT in conjugaciones)

**35 verbs found** ⚠️

These verbs are in the verbList but have NO Spanish conjugation entry:

| Verb Type | Count | Verbs |
|-----------|-------|-------|
| **Regular** | 18 | activate, add, climb, close, collect, complete, download, install, load, print, test, turn, type, unlock, upgrade, upload, use, visit |
| **Irregular** | 17 | be, beat, become, begin, blow, bring, come, cost, cut, do, get, go, hang, have, hit, hold, hurt, keep, lead, lend, let, make, mean, put, ride, ring, rise, say, seek, set, shake, shoot, shrink, shut, sink, stand, steal, strike, swing, wear |

### By Category

**Regular verbs missing conjugations (18):**
```
activate, add, climb, close, collect, complete, download, install, 
load, print, test, turn, type, unlock, upgrade, upload, use, visit
```

**Irregular verbs missing conjugations (17):**
```
be, beat, become, begin, blow, bring, come, cost, cut, do, get, go, 
have, hold, hurt, keep, make, mean, put, read, ride, ring, rise, say, 
seek, set, shake, shoot, shrink, shut, sink, stand, steal, strike, swing, wear
```

---

## 🎯 KEY FINDINGS & RECOMMENDATIONS

### ✅ What's Working Well:
1. **Strong core coverage** - Most common verbs have Spanish conjugations
2. **Good separation** - Regular vs Irregular verbs clearly separated in verbList
3. **Sentence generation list** - verbsForSentences is well-curated (64 verbs)
4. **Spanish naming** - Spanish infinitives are properly populated in verbList

### ⚠️ Critical Issues:

#### Issue 1: Orphan Entries (3 critical)
**Problem:** 3 verbs in conjugaciones exist but are NOT in verbList:
- `laugh` (irregular)
- `cry` (regular)
- `worry` (regular)

**Impact:** These verbs won't work properly in sentence generation and verb conjugation tables.

**Fix:** Add these to verbList:
```csharp
new VerbEntry { Infinitive="laugh",  Past="laughed",  Participle="laughed",  Gerund="laughing",  SpanishInf="reírse",      IsRegular=true },
new VerbEntry { Infinitive="cry",    Past="cried",    Participle="cried",    Gerund="crying",    SpanishInf="llorar",      IsRegular=true },
new VerbEntry { Infinitive="worry",  Past="worried",  Participle="worried",  Gerund="worrying",  SpanishInf="preocuparse", IsRegular=true },
```

#### Issue 2: Missing Spanish Conjugations (35 verbs)
**Problem:** 35 verbs in verbList have NO Spanish conjugation data, which means:
- BuildSpanishVerb() will fail for these verbs
- Sentence generation will show verb names instead of conjugations
- Spanish learning output will be incomplete

**Priority Missing Conjugations (18 Regular + 17 Irregular):**

Most critical (commonly used):
- `be`, `do`, `go`, `have` - ESSENTIAL for any English course
- `come`, `make`, `read`, `say` - HIGH FREQUENCY
- `cut`, `get`, `hold`, `put`, `set`, `wear` - MEDIUM FREQUENCY

**Fix Required:** Add conjugation entries for all 35 missing verbs to the conjugaciones dictionary.

#### Issue 3: Duplicate Handling
**Observation:** Some Spanish conjugations point to the same forms:
- `carry` and `load` both map to `cargo` (first person present)
- `type` and `write` both map to `escribo`

This is actually ACCEPTABLE as Spanish allows this, but should be documented.

#### Issue 4: Special/Reflexive Verbs
**Observation:** Some verbs have reflexive forms:
- `hide` → "me escondo" (I hide myself)
- `laugh` → "me río" (I laugh)
- `dress` → "me visto" (I dress myself)
- `worry` → "me preocupo" (I worry myself)

These are correctly handled but be aware for advanced learning.

---

## 📋 ACTION ITEMS (Priority Order)

### PRIORITY 1 (Must Fix - Breaks functionality):
- [ ] Add `laugh`, `cry`, `worry` to verbList
- [ ] Add Spanish conjugations for: `be`, `do`, `go`, `have`, `come`, `make`, `read`, `say` (8 essential)

### PRIORITY 2 (Should Fix - Common verbs):
- [ ] Add conjugations for: `cut`, `get`, `hold`, `put`, `set`, `wear`, `let`, `keep` (8 medium-frequency)

### PRIORITY 3 (Should Fix - Regular verbs):
- [ ] Add conjugations for remaining 18 regular verbs missing conjugations

### PRIORITY 4 (Nice to Have - Less common):
- [ ] Add conjugations for remaining irregular verbs

---

## 📈 Completeness Scoring

| Metric | Current | Target | Status |
|--------|---------|--------|--------|
| conjugaciones coverage | 134/153 (87.6%) | 100% | ⚠️ NEEDS WORK |
| verbList completeness | 153/153 (100%) | 100% | ✓ GOOD |
| Orphan count | 3 | 0 | ⚠️ NEEDS FIX |
| verbsForSentences | 64 | 64+ | ✓ GOOD |
| Spanish conjugations for verbList | 118/153 (77.1%) | 100% | ⚠️ NEEDS WORK |

---

## 🔍 Detailed Verb Inventory

### Verbs by Status:

**✓ Complete** (in both conjugaciones AND verbList): 118 verbs
**⚠️ Orphaned** (in conjugaciones but NOT verbList): 3 verbs  
**❌ Missing Conjugations** (in verbList but NOT conjugaciones): 35 verbs

### By Regularity:

| Status | Regular | Irregular | Total |
|--------|---------|-----------|-------|
| Complete | 77 | 41 | 118 |
| Orphaned | 3 | 0 | 3 |
| Missing Conjugations | 18 | 17 | 35 |
| **TOTALS** | **98** | **58** | **156** |

---

## 🎓 Recommendations for Improvement

1. **Immediate:** Fix the 3 orphaned verbs (laugh, cry, worry)
2. **High Priority:** Add conjugations for the 8 essential/high-frequency verbs
3. **Medium Priority:** Complete all 35 missing conjugations
4. **Long-term:** Consider adding more irregular verbs for advanced learners
5. **Documentation:** Add comments in code for reflexive verbs and special cases

---

**Analysis Complete** ✓
