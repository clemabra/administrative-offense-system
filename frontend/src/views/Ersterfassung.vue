<template>
  <div class="container">
    <img id="background" src="@/assets/landesverwaltungsamt.jpeg" alt="background">
    <router-link to="/ordnungswidrigkeiten-verwalten" class="back-arrow">
      <i class="fas fa-arrow-left"></i>
    </router-link>
    <h1 id="title">Ordnungswidrigkeit nach §121 Abs.1 Nr.6 SGB XI</h1>
    <h2 id="mode">* Ersterfassung *</h2>
    <Eingabeformular @submit="handleSubmit"/>
    <p id="response" style="text-align: center; font-weight: bold" v-if="responseMessage">{{ responseMessage }}</p>
    
    <WarningPopUp ref="warningPopUp"/>
  </div>
</template>

<script setup>
import Eingabeformular from '../components/Eingabeformular.vue'
import { getApiUrl } from '@/api'
import { authFetch } from '@/utils/authFetch'
import WarningPopUp from '../components/WarningPopUp.vue'
</script>

<script>
export default {
  data() {
    return {
      responseMessage: '',
      isSubmitting: false
    }
  },
  methods: {
    async handleSubmit(formData) {
      if (this.isSubmitting) return
      this.isSubmitting = true

      try {
        const updatedFormData = { ...formData, rowVersion: 0 }

        console.log('Sending data:', updatedFormData)

        const response = await authFetch(getApiUrl('/Offense'), {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(updatedFormData)
        })

        if (!response.ok) {
          const errData = await response.json()
          console.error('API error:', errData)
          
          this.responseMessage = errData.message || 'Fehler beim Speichern der Ordnungswidrigkeit'
          
          // send popup
          if (errData.message == "Datenkombination aus VU-Nummer, Versicherungsnummer und Beginn Rückstand existiert bereits.")
            this.$refs.warningPopUp.showWarning(errData.message)

          if (errData.message == "Fehler beim Generieren des Weiserzeichens.")
            this.$refs.warningPopUp.showWarning(errData.message)

          return
        }

        const result = await response.json()
        this.responseMessage = result.message || 'Ordnungswidrigkeit erfolgreich gespeichert'
      } catch (error) {
        console.error('API error:', error)
        this.responseMessage = 'Fehler beim Speichern der Ordnungswidrigkeit'
      } finally {
        this.isSubmitting = false
      }
    }
  }
}
</script>

<style scoped>
#background {
  height: 100%;
  width: 100%;
  z-index: -1;
}

#title {
  font-size: 30px;
  text-align: center;
}

.back-arrow {
  position: relative;
  left: 50px;
  top: 50px;
  color: #333;
  text-decoration: none;
  z-index: 100;
}

.back-arrow i {
  font-size: 24px;
  transition: all 0.3s ease;
}

.back-arrow i:hover {
  color: #E8C325;
  scale: 1.1;
}

#arrow-icon {
  display: none;
}

#mode {
  color: rgb(222, 38, 38);
  text-align: center;
  margin-bottom: -0.5em;
}
</style>
