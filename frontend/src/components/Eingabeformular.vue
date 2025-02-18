/*
 * OPAS - Input Form Component
 * Copyright (C) 2025 Ims2425Bruh
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

<template>
  <div id="input-form-container">
    <div id="input-form">
      <form @submit.prevent="handleSubmit" novalidate>

        <table id="formal-table">
          <tr>
            <th colspan="11">Formelle Angaben</th>
          </tr>

          <tr>
            <td colspan="5" style="padding-right: 0;">
              <label for="referatsnummer" class="field-label">Aktenzeichen</label>

              <div style="display: inline-flex; align-items: center;">
                <input
                  type="text"
                  id="referatsnummer"
                  required
                  value="504.1"
                  :class="{ 'error-border': errorMessages.referatsnummer }"
                  class="readonly-field"
                  readonly
                >
                <span style="margin: 0 5px 0 6px">.</span>
              </div>
            </td>

            <td colspan="4" style="padding-right: 0;">
              <label for="weiserzeichen" class="field-label" style="color: transparent">Wz.</label>

              <div style="display: inline-flex; align-items: center;">
                <input
                  type="text"
                  id="weiserzeichen"
                  @focusout="validateData('weiserzeichen')"
                  v-model="weiserzeichen"
                  placeholder="Wz."
                  maxlength="2"
                  :class="{ 'error-border': errorMessages.weiserzeichen }"
                >
                <span style="margin: 0 5px 0 6px">/</span>
              </div>
            </td>

            <td colspan="10">
              <label for="fallnummer" class="field-label" style="color: transparent">Fallnummer</label>
              <input
                type="text"
                id="fallnummer"
                required
                placeholder="..."
                title="Die Fallnummer wird beim Speichern automatisch generiert."
                @focusout="validateData('fallnummer')"
                v-model="fallnummer"
                :class="{ 'error-border': errorMessages.fallnummer }"
                class="readonly-field"
                readonly
              >
              
            </td>

            <td colspan="19">
              <label for="meldedatum" class="field-label">Meldedatum*</label>
              <input
                type="date"
                id="meldedatum"
                required
                @focusout="validateData('meldedatum')"
                v-model="meldedatum"
                :class="{ 'error-border': errorMessages.meldedatum }"
              >
              <div class="error-container" v-if="errorMessages.meldedatum">
                <span class="error-message">{{ errorMessages.meldedatum }}</span>
              </div>
            </td>
          </tr>
          <th colspan="20">
            <div class="error-container" v-if="errorMessages.referatsnummer || errorMessages.weiserzeichen || errorMessages.fallnummer">
              <span v-if="errorMessages.referatsnummer" class="error-message">{{ errorMessages.referatsnummer }}</span>
              <span v-if="errorMessages.weiserzeichen" class="error-message">{{ errorMessages.weiserzeichen }}</span>
              <span v-if="errorMessages.fallnummer" class="error-message">{{ errorMessages.fallnummer }}</span>
            </div>
          </th>
        </table>

        <table id="insurance-table">
          <tr>
            <th colspan="2">Angaben zur Versicherung</th>
          </tr>
          <tr>
            <td>
              <label for="vu-nr" class="field-label">VU-Nr.*</label>
              <input
                type="text"
                id="vu-nr"
                required
                @focusin="handleImportantFieldFocusIn('versicherungsunternehmensnummer')"
                @focusout="handleImportantFieldFocusOut('versicherungsunternehmensnummer')"
                v-model="versicherungsunternehmensnummer"
                placeholder="VU-Nr."
                :class="{ 'error-border': errorMessages.versicherungsunternehmensnummer }"
              >
              <div class="error-container" v-if="errorMessages.versicherungsunternehmensnummer">
                <span class="error-message">{{ errorMessages.versicherungsunternehmensnummer }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.versicherungsunternehmensnummer">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.versicherungsunternehmensnummer }}
                </span>
              </div>
            </td>
            <td>
              <label for="krankenversicherung" class="field-label">Krankenversicherung*</label>
              <input
                type="text"
                id="krankenversicherung"
                required
                @focusout="validateData('krankenversicherung')"
                v-model="krankenversicherung"
                placeholder="Krankenversicherung"
                :class="{ 'error-border': errorMessages.krankenversicherung }"
              >
              <div class="error-container" v-if="errorMessages.krankenversicherung">
                <span class="error-message">{{ errorMessages.krankenversicherung }}</span>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <label for="versicherungsnummer" class="field-label">Versicherungsnummer*</label>
              <input
                type="text"
                id="versicherungsnummer"
                required
                v-model="versicherungsnummer"
                @focusin="handleImportantFieldFocusIn('versicherungsnummer')"
                @focusout="handleImportantFieldFocusOut('versicherungsnummer')"
                placeholder="Versicherungsnummer"
                :class="{ 'error-border': errorMessages.versicherungsnummer }"
              >
              <div class="error-container" v-if="errorMessages.versicherungsnummer">
                <span class="error-message">{{ errorMessages.versicherungsnummer }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.versicherungsnummer">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.versicherungsnummer }}
                </span>
              </div>
            </td>
            <td>
              <label for="beginn-versicherung" class="field-label">Beginn Versicherung*</label>
              <input
                type="date"
                id="beginn-versicherung"
                required
                @focusout="validateData('beginnVersicherung')"
                v-model="beginnVersicherung"
                :class="{ 'error-border': errorMessages.beginnVersicherung }"
              >
              <div class="error-container" v-if="errorMessages.beginnVersicherung">
                <span class="error-message">{{ errorMessages.beginnVersicherung }}</span>
              </div>
            </td>
          </tr>
        </table>

        <table id="person-table">
          <tr>
            <th colspan="3">Angaben zur beschuldigten Person</th>
          </tr>
          <tr>
            <td>
              <label for="geschlecht" class="field-label">Geschlecht*</label>
              <select
                id="geschlecht"
                required
                v-model="geschlecht"
                @focusout="validateData('geschlecht')"
                :class="{ 'error-border': errorMessages.geschlecht }"
              >
                <option value="" ref="empty" disabled selected hidden>Geschlecht</option>
                <option value="0" ref="no-declaration"></option>
                <option value="1">Herr</option>
                <option value="2">Frau</option>
                <option value="3">Divers</option>
              </select>
              <div class="error-container" v-if="errorMessages.geschlecht">
                <span class="error-message">{{ errorMessages.geschlecht }}</span>
              </div>
            </td>
            <td>
              <label for="titel" class="field-label">Titel</label>
              <input
                type="text"
                id="titel"
                v-model="titel"
                placeholder="Titel"
              >
            </td>
            <td>
              <label for="geburtsdatum" class="field-label">Geburtsdatum*</label>
              <input
                type="date"
                id="geburtsdatum"
                required
                @focusin="handleImportantFieldFocusIn('geburtsdatum')"
                @focusout="handleImportantFieldFocusOut('geburtsdatum')"
                v-model="geburtsdatum"
                :class="{ 'error-border': errorMessages.geburtsdatum }"
              >
              <div class="error-container" v-if="errorMessages.geburtsdatum">
                <span class="error-message">{{ errorMessages.geburtsdatum }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.geburtsdatum">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.geburtsdatum }}
                </span>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <label for="vorname" class="field-label">Vorname*</label>
              <input
                type="text"
                id="vorname"
                required
                @focusin="handleImportantFieldFocusIn('vorname')"
                @focusout="handleImportantFieldFocusOut('vorname')"
                v-model="vorname"
                placeholder="Vorname"
                :class="{ 'error-border': errorMessages.vorname }"
              >
              <div class="error-container" v-if="errorMessages.vorname">
                <span class="error-message">{{ errorMessages.vorname }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.vorname">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.vorname }}
                </span>
              </div>
            </td>
            <td>
              <label for="nachname" class="field-label">Nachname*</label>
              <input
                type="text"
                id="nachname"
                required
                @focusin="handleImportantFieldFocusIn('nachname')"
                @focusout="handleImportantFieldFocusOut('nachname')"
                v-model="nachname"
                placeholder="Nachname"
                :class="{ 'error-border': errorMessages.nachname }"
              >
              <div class="error-container" v-if="errorMessages.nachname">
                <span class="error-message">{{ errorMessages.nachname }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.nachname">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.nachname }}
                </span>
              </div>
            </td>
            <td>
              <label for="geburtsname" class="field-label">Geburtsname</label>
              <input
                type="text"
                id="geburtsname"
                v-model="geburtsname"
                placeholder="Geburtsname"
              >
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <label for="str" class="field-label">Straße*</label>
              <input
                type="text"
                id="str"
                required
                @focusout="validateData('str')"
                v-model="str"
                placeholder="Straße"
                :class="{ 'error-border': errorMessages.str }"
              >
              <div class="error-container" v-if="errorMessages.str">
                <span class="error-message">{{ errorMessages.str }}</span>
              </div>
            </td>
            <td>
              <label for="hausnummer" class="field-label">Hausnummer*</label>
              <input
                type="text"
                id="hausnummer"
                required
                @focusout="validateData('hausnummer')"
                v-model="hausnummer"
                placeholder="Hausnummer"
                :class="{ 'error-border': errorMessages.hausnummer }"
              >
              <div class="error-container" v-if="errorMessages.hausnummer">
                <span class="error-message">{{ errorMessages.hausnummer }}</span>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <label for="plz" class="field-label">PLZ*</label>
              <input
                type="text"
                id="plz"
                required
                @input="handleNumOnlyInput('plz')"
                @focusout="validateData('plz')"
                v-model="plz"
                placeholder="PLZ"
                :class="{ 'error-border': errorMessages.plz }"
              >
              <div class="error-container" v-if="errorMessages.plz">
                <span class="error-message">{{ errorMessages.plz }}</span>
              </div>
            </td>
            <td colspan="2">
              <label for="wohnort" class="field-label">Wohnort*</label>
              <input
                type="text"
                id="wohnort"
                required
                @focusout="validateData('wohnort')"
                v-model="wohnort"
                placeholder="Wohnort"
                :class="{ 'error-border': errorMessages.wohnort }"
              >
              <div class="error-container" v-if="errorMessages.wohnort">
                <span class="error-message">{{ errorMessages.wohnort }}</span>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <label for="ortsteil" class="field-label">Ortsteil</label>
              <input
                type="text"
                id="ortsteil"
                v-model="ortsteil"
                placeholder="Ortsteil"
              >
            </td>
          </tr>
          <tr>
            <td colspan="2">
              <label for="geburtsort" class="field-label">Geburtsort</label>
              <input
                type="text"
                id="geburtsort"
                v-model="geburtsort"
                placeholder="Geburtsort"
              >
            </td>
          </tr>
        </table>

        <table id="offense-table">
          <tr>
            <th colspan="2">Angaben zum Tatbestand § 51 Abs.1 Satz 2 SGB XI</th>
          </tr>
          <tr>
            <td>
              <label for="aufforderungsdatum" class="field-label">Aufforderungsdatum*</label>
              <input
                type="date"
                id="aufforderungsdatum"
                required
                v-model="aufforderungsdatum"
                @focusout="validateData('aufforderungsdatum')"
                :class="{ 'error-border': errorMessages.aufforderungsdatum || errorMessages.timelineError }"
              >
              <div class="error-container" v-if="errorMessages.aufforderungsdatum">
                <span class="error-message">{{ errorMessages.aufforderungsdatum }}</span>
              </div>
            </td>
            <td>
              <label for="beginn-rückstand" class="field-label">Beginn Rückstand*</label>
              <input
                type="month"
                id="beginn-rückstand"
                required
                v-model="beginnRueckstand"
                @focusin="handleImportantFieldFocusIn('beginnRueckstand')"
                @focusout="handleImportantFieldFocusOut('beginnRueckstand')"
                :class="{ 'error-border': errorMessages.beginnRueckstand  || errorMessages.timelineError }"
              >
              <div class="error-container" v-if="errorMessages.beginnRueckstand">
                <span class="error-message">{{ errorMessages.beginnRueckstand }}</span>
              </div>
              <div class="warning-container" v-if="warningMessages.beginnRueckstand">
                <span class="warning-message">
                  <i class="fas fa-exclamation-triangle"></i>
                  {{ warningMessages.beginnRueckstand }}
                </span>
              </div>
            </td>
          </tr>

          <tr>
            <td colspan="2">
              <div class="error-container" v-if="errorMessages.timelineError">
                <span class="error-message">{{ errorMessages.timelineError }}</span>
              </div>
            </td>
          </tr>

          <tr>
            <td>
              <label for="verzug-bis" class="field-label">Verzug bis</label>
              <input
                type="text"
                id="verzug-bis"
                required
                v-model="formattedVerzugBis"
                class="readonly-field"
                readonly
              >
            </td>
            <td>
              <label for="verzugsende" class="field-label">Verzugsende</label>
              <input
                type="date"
                id="verzugsende"
                v-model="verzugsende"
                @focusout="validateData('verzugsende')"
                :class="{ 'error-border': errorMessages.verzugsende }"
              >
              <div class="error-container" v-if="errorMessages.verzugsende">
                <span class="error-message">{{ errorMessages.verzugsende }}</span>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <label for="beitragsrueckstand" class="field-label">Beitragsrückstand*</label>
              <div class="input-euro-wrapper">
                <input
                  type="text"
                  id="beitragsrueckstand"
                  required
                  v-model="beitragsrueckstand"
                  @focusout="validateData('beitragsrueckstand')"
                  placeholder="Beitragsrückstand"
                  :class="{ 'error-border': errorMessages.beitragsrueckstand }"
                >
                <span class="euro-symbol">€</span>
              </div>
              <div class="error-container" v-if="errorMessages.beitragsrueckstand">
                <span class="error-message">{{ errorMessages.beitragsrueckstand }}</span>
              </div>
            </td>
            <td>
              <label for="sollbeitrag" class="field-label">Sollbeitrag*</label>
              <div class="input-euro-wrapper">
                <input
                  type="text"
                  id="sollbeitrag"
                  required
                  @focusout="validateData('sollbeitrag')"
                  v-model="sollbeitrag"
                  placeholder="Sollbeitrag"
                  :class="{ 'error-border': errorMessages.sollbeitrag }"
                >
                <span class="euro-symbol">€</span>
              </div>
              <div class="error-container" v-if="errorMessages.sollbeitrag">
                <span class="error-message">{{ errorMessages.sollbeitrag }}</span>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="1">
              <label for="folgemeldung" class="field-label">Folgemeldung*</label>
              <input
                type="text"
                id="folgemeldung"
                required
                v-model="folgemeldung"
                @input="handleNumOnlyInput('folgemeldung')"
                @focusout="validateData('folgemeldung')"
                placeholder="Folgemeldung"
                :class="{ 'error-border': errorMessages.folgemeldung }"
              >
              <div class="error-container" v-if="errorMessages.folgemeldung">
                <span class="error-message">{{ errorMessages.folgemeldung }}</span>
              </div>
            </td>
          </tr>
        </table>

        <table id="additional-table">
          <tr>
            <th>Weitere Angaben</th>
          </tr>
          <tr>
            <td>
              <textarea
                id="bemerkungen"
                v-model="bemerkungen"
                placeholder="Bemerkungen"
              ></textarea>
            </td>
          </tr>
        </table>

        <table id="documents-table">
          <tr>
            <th colspan="2">Dokumente</th>
          </tr>
          <tr>
            <td colspan="1">
              <label for="anhoehrungsdatum" class="field-label">Anhörungsdatum</label>
              <input
                type="date"
                id="anhoerungsdatum"
                v-model="anhoerungsdatum"
              >
            </td>
          </tr>
        </table>

        <div id="submit-and-response">
          <button
            id="cancel-button"
            type="button"
            @click="handleCancel"
            aria-label="cancel-button"
          >Abbrechen
          </button>
          <button
            id="fill-button"
            type="button"
            @click="fillFormFields"
            aria-label="fill-button"
          >Formular füllen <br> (für Testzwecke)
          </button>
          <button
            id="submit-button"
            aria-label="send-button"
          >{{ sendMode }}
          </button>
        </div>
        <ImportantChangesPopUp ref="warningPopUp"/>
      </form>
    </div>
  </div>
</template>

<script setup>
import '@fortawesome/fontawesome-free/css/all.css'
import ImportantChangesPopUp from './ImportantChangesPopUp.vue'
</script>

<script>
export default {
  props: {
    initialData: {
      type: Object,
      default: () => ({})
    }
  },
  data() {
    return {
      weiserzeichen: this.initialData.weiserzeichen || '',
      fallnummer: this.initialData.fallnummer || '',
      meldedatum: this.formatDate(this.initialData.meldedatum) || '',
      versicherungsunternehmensnummer: this.initialData.versicherungsunternehmensnummer || '',
      krankenversicherung: this.initialData.krankenversicherung || '',
      versicherungsnummer: this.initialData.versicherungsnummer || '',
      beginnVersicherung: this.formatDate(this.initialData.beginnVersicherung) || '',
      geschlecht: this.initialData.geschlecht || '',
      titel: this.initialData.titel || '',
      geburtsdatum: this.formatDate(this.initialData.geburtsdatum) || '',
      vorname: this.initialData.vorname || '',
      nachname: this.initialData.nachname || '',
      geburtsname: this.initialData.geburtsname || '',
      str: this.initialData.str || '',
      hausnummer: this.initialData.hausnummer || '',
      plz: this.initialData.plz || '',
      wohnort: this.initialData.wohnort || '',
      ortsteil: this.initialData.ortsteil || '',
      geburtsort: this.initialData.geburtsort || '',
      aufforderungsdatum: this.formatDate(this.initialData.aufforderungsdatum) || '',
      beginnRueckstand: this.convertToMonth(this.initialData.beginnRueckstand) || '',
      verzugBis: this.formatDate(this.initialData.verzugBis) || '',
      verzugsende: this.formatDate(this.initialData.verzugsende) || null,
      beitragsrueckstand: this.initialData.beitragsrueckstand || '',
      sollbeitrag: this.initialData.sollbeitrag || '',
      folgemeldung: this.initialData.folgemeldung || '',
      bemerkungen: this.initialData.bemerkungen || '',
      anhoerungsdatum: this.formatDate(this.initialData.anhoerungsdatum) || null,

      errorMessages: {},
      warningMessages: {},
      sendMode: this.initialData.fallnummer ? 'Speichern' : 'Absenden', // if fallnummer is set, we're in edit mode
      isSubmitting: false // prevent multiple submissions
    }
  },
  methods: {
    formatDate(dateString) {
      if (!dateString || dateString === '0001-01-01T00:00:00' || dateString === '') return null
      
      // default date is 0001-01-01, return null if this is the case (for optional fields)
      const date = new Date(dateString)
      if (date.getFullYear() === 1 && date.getMonth() === 0 && date.getDate() === 1) {
        return null
      }

      const year = date.getFullYear()
      const month = (date.getMonth() + 1).toString().padStart(2, '0')
      const day = date.getDate().toString().padStart(2, '0')
      return `${year}-${month}-${day}`
    },
    updateVerzugBis() {
      if (this.beginnRueckstand) {
        const date = new Date(this.beginnRueckstand)
        let year = date.getFullYear()
        let month = date.getMonth()

        // handles overflow of 12 months
        month += 5
        if (month > 11) {
          // year + 1 for each 12 months
          year += Math.floor(month / 12)
          // rest of the months
          month %= 12
        }

        const newDate = new Date(year, month, 1)
        // increment day since it always registers the last one of the previous month
        newDate.setDate(newDate.getDate() + 1)
        this.verzugBis = newDate.toISOString().substring(0, 10)
      }
    },
    convertToMonth(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString)
      const year = date.getFullYear()
      const month = (date.getMonth() + 1).toString().padStart(2, '0')
      return `${year}-${month}`
    },
    convertToDate(dateString) {
      if (!dateString) return ''
      const date = new Date(dateString + '-01')
      const year = date.getFullYear()
      const month = (date.getMonth() + 1).toString().padStart(2, '0')
      const day = date.getDate().toString().padStart(2, '0')
      return `${year}-${month}-${day}`
    },
    // async fillWeiserzeichen() {
    //   try {
    //     const response = await authFetch(getApiUrl('/Offense'))
    //     const result = await response.json()

    //     this.items = result.data
    //     // console.log(this.items)

    //     const usedValues = new Set(this.items.map(item => item.weiserzeichen).filter(value => value));
    //     let nextValue = 1
        
    //     while (usedValues.has(String(nextValue))) {
    //       nextValue++ // Increment until a unique value is found
    //     }
    //     this.weiserzeichen = String(nextValue)
    //     // console.log(nextValue)

    //   } catch (error) {
    //     console.error('Error fetching items:', error)
    //   }
    // },
    validateData(fieldName, isSubmit = false) {
      const currDate = new Date()
      let errorMessage = ''

      switch (fieldName) {
        case 'weiserzeichen':
          if (!this.weiserzeichen && isSubmit && this.sendMode === 'Speichern')
            errorMessage = 'Bitte geben Sie das Weiserzeichen ein.'
          if (this.weiserzeichen.length > 2)
            errorMessage = 'Das Weiserzeichen darf maximal 2 Zeichen lang sein.'
          break
        case 'fallnummer':
          // no initial fn
          if (this.initialData.fallnummer && !this.fallnummer ||
              this.initialData.fallnummer && this.errorMessages.fallnummer
          )
            errorMessage = 'Bei der Generierung der Fallnummer ist ein Fehler aufgetreten.'
          break

          /* OBSOLETE since fn gets generated by API

          // initial fn but no new fn when sending
          if (!this.fallnummer)
            this.errorMessages.fallnummer = 'Bei der Generierung der Fallnummer ist ein Fehler aufgetreten.'
          
          // if fn only base numer
          if (!this.fallnummer.includes('-')) {
            // check for D5 format
            if (!firstPart.length !== 5)
              this.errorMessages.fallnummer = 'Der Hauptteil der Fallnummer muss aus genau 5 Ziffern bestehen'
          }
          // if fn includes '-' check second part
            const secondPart = this.fallnummer.split('-')[1]
            
            // check if second part is a number
            if (secondPart.length < 1 || secondPart.length > 4)
              this.errorMessages.fallnummer = 'Das Suffix der Fallnummer muss zwischen 1 und 4 Ziffern lang sein.'

            // check if second part starts with 0
            if (secondPart.get(0) === '0')
              this.errorMessages.fallnummer = 'Das Suffix der Fallnummer darf nicht mit 0 beginnen.'

            break
          */

        case 'meldedatum':
          if (!this.meldedatum && isSubmit)
            errorMessage = 'Bitte geben Sie das Meldedatum ein.'
          if (new Date(this.meldedatum) > currDate)
            errorMessage = 'Das Meldedatum darf nicht in der Zukunft liegen.'
          break
        case 'versicherungsunternehmensnummer':
          if (!this.versicherungsunternehmensnummer && isSubmit)
            errorMessage = 'Bitte geben Sie die Versicherungsunternehmensnummer an.'
          break
        case 'krankenversicherung':
          if (!this.krankenversicherung && isSubmit)
            errorMessage = 'Bitte geben Sie eine Krankenversicherung an.'
          break
        case 'versicherungsnummer':
          if (!this.versicherungsnummer && isSubmit)
            errorMessage = 'Bitte geben Sie die Versicherungsnummer an.'
          break
        case 'beginnVersicherung':
          if (!this.beginnVersicherung && isSubmit)
            errorMessage = 'Bitte geben Sie den Beginn der Versicherung an.'
          if (new Date(this.beginnVersicherung) > currDate)
            errorMessage = 'Der Beginn der Versicherung darf nicht in der Zukunft liegen.'
          break
        case 'geschlecht':
          if (!(this.geschlecht || this.geschlecht === '0') && isSubmit)
            errorMessage = 'Bitte geben Sie das Geschlecht an.'
          break
        case 'geburtsdatum':
          if (!this.geburtsdatum && isSubmit)
            errorMessage = 'Bitte geben Sie das Geburtsdatum an.'
          if (new Date(this.geburtsdatum) > currDate)
            errorMessage = 'Das Geburtsdatum darf nicht in der Zukunft liegen.'
          break
        case 'vorname':
          if (!this.vorname && isSubmit)
            errorMessage = 'Bitte geben Sie den Vornamen an.'
          break
        case 'nachname':
          if (!this.nachname && isSubmit)
            errorMessage = 'Bitte geben Sie den Nachnamen an.'
          break
        case 'str':
          if (!this.str && isSubmit)
            errorMessage = 'Bitte geben Sie die Straße an.'
          break
        case 'hausnummer':
          if (!this.hausnummer && isSubmit)
            errorMessage = 'Bitte geben Sie die Hausnummer an.'
          if (this.hausnummer.startsWith('0'))
            errorMessage = 'Die Hausnummer darf nicht mit 0 beginnen.'
          break
        case 'plz':
          if (!this.plz && isSubmit) {
            errorMessage = 'Bitte geben Sie die PLZ an.'
            break
          } else if (this.plz.length !== 5 && this.plz.length !== 0)
            errorMessage = 'Die PLZ muss genau 5 Ziffern lang sein.'
          break
        case 'wohnort':
          if (!this.wohnort && isSubmit)
            errorMessage = 'Bitte geben Sie den Wohnort an.'
          break
        case 'aufforderungsdatum':
          if (!this.aufforderungsdatum && isSubmit)
            errorMessage = 'Bitte geben Sie das Aufforderungsdatum an.'
          if (new Date(this.aufforderungsdatum) > currDate)
            errorMessage = 'Das Aufforderungsdatum darf nicht in der Zukunft liegen.'

          if (!this.beginnRueckstand || !this.aufforderungsdatum && this.errorMessages.timelineError) {
            this.errorMessages.timelineError = ''
            break
          }

          if (this.beginnRueckstand && this.aufforderungsdatum && this.errorMessages.timelineError) {
            if (new Date(this.beginnRueckstand) <= new Date(this.aufforderungsdatum))
              this.errorMessages.timelineError = ''
          }
          break
        case 'beginnRueckstand':
          if (!this.beginnRueckstand && isSubmit) {
            errorMessage = 'Bitte geben Sie den Beginn des Rückstands an.'
            break
          }

          const bR = new Date(this.beginnRueckstand)
          if (bR > currDate) {
            errorMessage = 'Das Beginndatum des Rückstands darf nicht in der Zukunft liegen.'
            break
          } else if (bR.getDate() !== 1 && isSubmit) {
            errorMessage = 'Beginn Rückstand muss am Monatsanfang liegen.'
            break
          }

          if (!this.beginnRueckstand || !this.aufforderungsdatum && this.errorMessages.timelineError) {
            this.errorMessages.timelineError = ''
            break
          } else if (this.beginnRueckstand && this.aufforderungsdatum && this.errorMessages.timelineError) {
            if (new Date(this.beginnRueckstand) <= new Date(this.aufforderungsdatum))
              this.errorMessages.timelineError = ''
          }
          break
        case 'verzugBis':
          if (!this.verzugBis)
            errorMessage = 'Es gab einen Fehler beim Berechnen des Feldes "Verzug bis".'
          break
        case 'verzugsende':
          if (new Date(this.verzugsende) > currDate)
            errorMessage = 'Das Verzugsende darf nicht in der Zukunft liegen.'
          break
        case 'beitragsrueckstand':
          if (!this.beitragsrueckstand && isSubmit) {
            errorMessage = 'Bitte geben Sie den Beitragsrückstand an.'
            break
          } else if (this.beitragsrueckstand) {
            errorMessage = this.validateEuroField(this.beitragsrueckstand, 'Beitragsrückstand')
            break
          }
          break
        case 'sollbeitrag':
          if (!this.sollbeitrag && isSubmit) {
            errorMessage = 'Bitte geben Sie den Sollbeitrag an.'
            break
          } else if (this.sollbeitrag) {
            errorMessage = this.validateEuroField(this.sollbeitrag, 'Sollbeitrag')
            break
          }
          break
        case 'folgemeldung':
          if (!this.folgemeldung && this.folgemeldung !== 0 && isSubmit) {
            errorMessage = 'Bitte geben Sie die Folgemeldung an.'
            break
          }
          if (this.folgemeldung.length !== 0) {
            const folgemeldungValue = Number(this.folgemeldung)
            if (isNaN(folgemeldungValue) || !Number.isInteger(folgemeldungValue)) {
              errorMessage = 'Die Folgemeldung muss eine Ganzzahl sein.'
              break
            } else if (folgemeldungValue < 1) {
              errorMessage = 'Der Wert für die Folgemeldung muss mindestens 1 sein.'
            }
          }
          break
        default:
          break
      }

      if (errorMessage || this.errorMessages[fieldName]) {
        this.errorMessages[fieldName] = errorMessage
      }
    },
    validateAllData() {
      const fieldsToValidate = [
        'weiserzeichen',
        'meldedatum',
        'versicherungsunternehmensnummer',
        'krankenversicherung',
        'versicherungsnummer',
        'beginnVersicherung',
        'geschlecht',
        'geburtsdatum',
        'vorname',
        'nachname',
        'str',
        'hausnummer',
        'plz',
        'wohnort',
        'aufforderungsdatum',
        'beginnRueckstand',
        'verzugsende',
        'beitragsrueckstand',
        'sollbeitrag',
        'folgemeldung'
      ]

      this.errorMessages = {}
      fieldsToValidate.forEach((field) => this.validateData(field, true))

      // additional check if aufforderungsdatum is before beginnRueckstand -> only on send
      if (this.beginnRueckstand && this.aufforderungsdatum) {
        const brDate = new Date(this.beginnRueckstand)
        const adDate = new Date(this.aufforderungsdatum)
        if (brDate > adDate)
          this.errorMessages.timelineError = "Das Aufforderungsdatum darf nicht vor dem Beginn des Rückstands liegen."
      }
    },
    async handleSubmit(event) {

      // if (!this.weiserzeichen) {
      //   await this.fillWeiserzeichen()
      // }

      if (event) {
        event.preventDefault()
        event.stopPropagation()
      }

      if (this.isSubmitting) return
      this.isSubmitting = true

      this.validateAllData()

      // check errs
      const hasErrors = Object.values(this.errorMessages).some(
          (msg) => msg && msg.trim() !== ''
      )

      if (hasErrors) {
        this.isSubmitting = false
        return
      }

      // check warnings
      const hasWarnings = Object.values(this.warningMessages).some(
          (msg) => msg && msg.trim() !== ''
      )

      if (hasWarnings) {
        const confirmed = await this.$refs.warningPopUp.showWarningConfirmation(
          "Es gibt Änderungen an wichtigen Feldern. Möchten Sie trotzdem fortfahren?"
        )

        if (!confirmed) {
          this.isSubmitting = false
          return
        }
      }

      const formData = {
        weiserzeichen: this.weiserzeichen || '',
        fallnummer: this.fallnummer || '',
        meldedatum: this.meldedatum,
        versicherungsunternehmensnummer: this.versicherungsunternehmensnummer,
        krankenversicherung: this.krankenversicherung,
        versicherungsnummer: this.versicherungsnummer,
        beginnVersicherung: this.beginnVersicherung,
        geschlecht: this.geschlecht,
        titel: this.titel,
        geburtsdatum: this.geburtsdatum,
        vorname: this.vorname,
        nachname: this.nachname,
        geburtsname: this.geburtsname,
        str: this.str,
        hausnummer: this.hausnummer,
        plz: this.plz,
        wohnort: this.wohnort,
        ortsteil: this.ortsteil,
        geburtsort: this.geburtsort,
        aufforderungsdatum: this.aufforderungsdatum,
        beginnRueckstand: this.convertToDate(this.beginnRueckstand),
        verzugBis: this.verzugBis,
        verzugsende: this.verzugsende,
        beitragsrueckstand: this.beitragsrueckstand,
        sollbeitrag: this.sollbeitrag,
        folgemeldung: this.folgemeldung,
        bemerkungen: this.bemerkungen,
        anhoerungsdatum: this.anhoerungsdatum
      }

      this.isSubmitting = false

      this.$emit('submit', formData)
    },
    handleCancel() {
      window.history.back()
    },
    validateEuroField(value, fieldName) {
      if (!value)
        return

      const validCharRegex = /^-?[\d,]+$/
      if (!validCharRegex.test(value))
        return `Der ${fieldName} muss eine Zahl sein.`
      if (!value.includes(','))
        return `Der ${fieldName} muss genau 2 Nachkommastellen enthalten.`

      const parts = value.split(',')
      if (parts.length !== 2 || parts.some(part => part.trim() === ''))
        return `Der ${fieldName} muss eine Zahl sein.`

      const [euros, cents] = parts.map(Number)
      if (isNaN(euros) || isNaN(cents))
        return `Der ${fieldName} muss eine Zahl sein.`

      const digitRegex = /^-?\d+,\d{2}$/
      if (!digitRegex.test(value))
        return `Der ${fieldName} muss genau 2 Nachkommastellen enthalten.`

      const totalValue = euros + cents / 100
      if (totalValue < 0)
        return `Der ${fieldName} darf nicht negativ sein.`
      else if (totalValue < 0.01)
        return `Der ${fieldName} darf nicht 0 sein.`

      return ''
    },
    fillFormFields() {
      const today = new Date()
      const formattedTodaysDate = today.toISOString().split('T')[0]
      const formattedMonth = today.toISOString().slice(0, 7)
      this.weiserzeichen = '12'
      this.meldedatum = formattedTodaysDate
      this.versicherungsunternehmensnummer = "12345"
      this.krankenversicherung = "Testversicherung"
      this.versicherungsnummer = "A123456789"
      this.beginnVersicherung = formattedTodaysDate
      this.geschlecht = "1"
      this.titel = "Dr."
      this.geburtsdatum = formattedTodaysDate
      this.vorname = "Max"
      this.nachname = "Mustermann"
      this.geburtsname = "Muster"
      this.str = "Musterstraße"
      this.hausnummer = "42a"
      this.plz = "12345"
      this.wohnort = "Musterstadt"
      this.ortsteil = "Musterortsteil"
      this.geburtsort = "Mustergeburtsort"
      this.aufforderungsdatum = formattedTodaysDate
      this.beginnRueckstand = formattedMonth
      this.verzugBis = formattedTodaysDate
      this.verzugsende = formattedTodaysDate
      this.beitragsrueckstand = "123,45"
      this.sollbeitrag = "123,45"
      this.folgemeldung = 1
      this.bemerkungen = "Testbemerk"
      this.anhoerungsdatum = formattedTodaysDate
    },
    handleNumOnlyInput(fieldName) {
      this[fieldName] = this[fieldName].replace(/[^0-9]/g, '')
    },
    handleImportantFieldFocusIn(fieldName) {
      // if Ersterfassung stop
      if (this.sendMode !== 'Speichern') return

      if (!this.warningMessages[fieldName]) {
        this.warningMessages[fieldName] = 'Änderungen an diesem Feld können die Zuweisung der Fallnummer beeinflussen.'
      }
    },
    handleImportantFieldFocusOut(fieldName) {
      // if Ersterfassung stop but also validate
      if (this.sendMode !== 'Speichern') {
        this.validateData(fieldName)
        return
      }

      // get values to check changes
      const initVal = this.initialData[fieldName]
      const currVal = this[fieldName]

      let formattedInitVal = initVal
      let formattedCurrVal = currVal

      // check if month, or just regular VU-Nr.
      if (fieldName === 'beginnRueckstand') {
        formattedInitVal = this.convertToMonth(initVal)
        formattedCurrVal = this.convertToMonth(currVal)
      }

      if (fieldName === 'geburtsdatum') {
        formattedInitVal = this.formatDate(initVal)
        formattedCurrVal = this.formatDate(currVal)
      }

      // reset warning
      if (formattedInitVal === formattedCurrVal) {
        this.warningMessages[fieldName] = ''
      } else {
        this.handleImportantFieldChangeWarning(fieldName)
      }

      // still call validation function since @focusout is overwritten now in html
      this.validateData(fieldName)
    },
    handleImportantFieldChangeWarning(fieldName) {
      // if Ersterfassung stop
      if (this.sendMode !== 'Speichern') return

      const initVal = this.initialData[fieldName]
      const currVal = this[fieldName]
      if (initVal !== currVal) {
        this.warningMessages[fieldName] = 'Änderungen an diesem Feld können die Zuweisung der Fallnummer beeinflussen.'
      }
    }
  },
  watch: {
    versicherungsunternehmensnummer(newValue, oldValue) {
      if (newValue !== oldValue) {
        this.handleImportantFieldChangeWarning('versicherungsunternehmensnummer')
      }
    },
    versicherungsnummer(newValue, oldValue) {
      if (newValue !== oldValue) {
        this.handleImportantFieldChangeWarning('versicherungsnummer')
      }
    },
    beginnRueckstand(newValue, oldValue) {
      if (!newValue) {
        this.verzugBis = ''
        this.handleImportantFieldFocusOut('beginnRueckstand')
        return
      }

      const formattedOld = this.convertToMonth(oldValue)
      const formattedNew = this.convertToMonth(newValue)

      if (formattedOld !== formattedNew) {
        this.handleImportantFieldChangeWarning('beginnRueckstand')
        this.updateVerzugBis()
      } else {
        this.handleImportantFieldFocusOut('beginnRueckstand')
      }
    }
  },
  computed: {
    formattedVerzugBis() {
      if (this.verzugBis) {
        const date = new Date(this.verzugBis)
        const day = date.getDate().toString().padStart(2, '0')
        const month = (date.getMonth() + 1).toString().padStart(2, '0')
        const year = date.getFullYear()
        return `${day}.${month}.${year}`
      }
      return ''
    }
  }
}
</script>

<style scoped>
#input-form-container {
  background: #fdfdfd;
  width: 100%;
  min-height: 520px;
}

#input-form {
  padding: 1% 6%;
  font-size: 20px;
  font-weight: bold;
  margin: 0;
  display: flex;
  justify-content: center;
}

input {
  border: none;
  box-sizing: border-box;
  outline: 0;
  position: relative;
  width: 100%;
  font-family: Arial, serif;
}

input[type="date"] {
  padding: .38rem;
  font-weight: 450;
  font-family: Arial, serif;
}

input[type="text"] {
  padding: .45rem;
  font-family: Arial, serif;
}

select {
  padding: .41rem;
  box-sizing: border-box;
  outline: none;
  position: relative;
  width: 100%;
  font-size: 16px;
  font-family: Arial, serif;
  border: solid 1px #272727;
}

select:invalid {
  color: gray;
}

input, .readonly-field {
  display: block;
  padding: 6px 10px;
  box-sizing: border-box;
  border: 1px solid #404040;
  width: 100%;
  font-size: 16px;
}

textarea {
  outline: 0;
}

table {
  margin-bottom: 18px;
  width: 650px;
  table-layout: fixed;
}

th, td {
  text-align: left;
  vertical-align: top;
  padding-right: 10px;
  padding-top: 10px;
}

form {
  background: #fdfdfd;
}

input, .readonly-field, input[type="date"] {
  display: block;
  padding: 6px 10px;
  box-sizing: border-box;
  border: 1px solid #404040;
  width: 100%;
  font-size: 16px;
  font-family: Arial, Helvetica, sans-serif;
}

.input-euro-wrapper {
  display: flex;
  align-items: center;
  position: relative;
}

.euro-symbol {
  position: absolute;
  right: 5%;
  font-size: 16px;
  color: #404040;
  pointer-events: none;
}

/* hides up and down arrows in fron of the euro symbol */
input[type="number"]::-webkit-outer-spin-button,
input[type="number"]::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

#bemerkungen {
  width: calc(100% - 20px);
  height: 125px;
  resize: none;
  font-family: Arial, Helvetica, sans-serif;
  font-size: 16px;
  padding: 6px 10px;
  border: 1px solid #404040;
}

.readonly-field {
  background: #f1f1f1;
  color: gray;
  font-family: Arial, Helvetica, sans-serif;
  font-weight: normal;
}

.field-label {
  font-size: 14px;
  margin-bottom: 15px;
  font-family: Arial, Helvetica, sans-serif;
  color: #0a1c2c;
}

.error-container, .warning-container {
  width: 100%;
  margin-top: 5px;
}

.error-message, .warning-message {
  color: red;
  font-size: 12px;
  word-wrap: break-word;
  display: block;
}

.error-border {
  border-color: red !important;
}

.warning-message {
  color: #E8C325;
  font-size: 12px;
  word-wrap: break-word;
  display: flex;
  align-items: center;
}

.warning-message i {
  color: #E8C325;
  margin-right: 5px;
  font-size: 14px;
}

#submit-and-response {
  text-align: center;
  padding-top: 3%;
}

#submit-button, #cancel-button {
  background: #404040;
  font-size: 20px;
  padding: 1% 3%;
  color: #fdfdfd;
  border: none;
  cursor: pointer;
  transition: 0.3s;
  margin: 0 1%;
}

#submit-button:disabled {
  background: grey;
  cursor: not-allowed;
}

#submit-button:hover:enabled {
  background: #E8C325;
}

#cancel-button:hover {
  background: #E8C325;
}

#fill-button {
  background: #4caf50;
  font-size: 16px;
  padding: 8px 16px;
  color: #ffffff;
  border: none;
  cursor: pointer;
  margin-bottom: 20px;
  transition: 0.3s;
}

#fill-button:hover {
  background: #45a049;
}

@media only screen and (max-width: 480px) {

  #submit-button {
    font-size: 16px;
    padding: 2% 4%;
  }

  th, td {
    display: inline-block;
  }
}
</style>