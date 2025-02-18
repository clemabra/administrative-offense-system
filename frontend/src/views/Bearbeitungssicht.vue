<template>
  <div id="container">
    <img id="background" src="@/assets/landesverwaltungsamt.jpeg" alt="background">

    <div class="snackbar-container">
      <div v-for="(snackbar, index) in snackbars"
        :key="index"
        :class="{'snackbar': true, 'error-message': snackbar.isError, 'success-message': !snackbar.isError}"
        style="position: relative; margin-top: 10px;"
      >
        <span class="snackbar-message">{{ snackbar.message }}</span>
        <button class="snackbar-close" @click="removeSnackbar(index)">&times;</button>
      </div>
    </div>

    <router-link to="/falluebersicht" class="back-arrow">
      <i class="fas fa-arrow-left"></i>
    </router-link>

    <h1 id="title">Ordnungswidrigkeit nach §121 Abs.1 Nr.6 SGB XI</h1>
    <h2 id="mode"> * Bearbeitungsmodus *</h2>
    <Eingabeformular :initialData="formData" @submit="handleSubmit"/>

    <WarningPopUp ref="warningPopUp"/>
  </div>
</template>

<script setup>
import Eingabeformular from '../components/Eingabeformular.vue'
import { getApiUrl } from '@/api'
import { authFetch } from '@/utils/authFetch'
import WarningPopUp from '../components/WarningPopUp.vue';
</script>

<script>
export default {
  props: {
    id: {
      type: String,
      required: true
    },
    data: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      responseMessage: '',
      isSubmitting: false,
      snackbars: [],
      warningMessages: {},
      localData: { ...this.data }
    }
  },
  methods: {
    async handleSubmit(formData) {
      // if blocking snackbar is present, do not submit
      if (this.snackbars.some(sb => sb.isBlocking)) return
      
      // prevent double submit
      if (this.isSubmitting) return
      this.isSubmitting = true

      try {
        const updatedFormData = {
          ...formData,
          recordId: this.localData.recordId,
          rowVersion: this.localData.rowVersion
        }
        const url = getApiUrl(`/Offense/${this.data.recordId}`)
        const response = await authFetch(url, {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(updatedFormData)
        })

        if (!response.ok) {
          const errorData = await response.json()

          // row version conflict
          if (response.status === 409) {
            if (errorData.message == "Speichern fehlgeschlagen. Die Ordnungswidrigkeit wurde von einem anderen Nutzer bereits gespeichert.") {
              this.$refs.warningPopUp.showWarning(errorData.message)
              return
            }
            else console.error('409 conflict:', errorData)
            await this.fetchLatestData()
            return
          }

          if (response.status === 500 && errorData.message) {
            // send popup
            if (errorData.message == "Datenkombination aus VU-Nummer, Versicherungsnummer und Beginn Rückstand existiert bereits.") {
              this.$refs.warningPopUp.showWarning(errorData.message)
              return
            }

            this.addSnackbar(errorData.message, true)
            return
          }

          throw new Error(`HTTP error! status: ${response.status}`)
        }

        const result = await response.json()
        this.localData.rowVersion = result.rowVersion
        this.addSnackbar(result.message)

        return result
      } catch (error) {
        this.addSnackbar(error.message, true)
      } finally {
        this.isSubmitting = false
      }
    },
    addSnackbar(message, isError = false, { isBlocking = false, errorCode = null } = {}) {
      // only one message of same type
      const existing = this.snackbars.find((sb) => sb.message === message)
      if (existing) return

      // remove all blocking snackbars
      if (isError)
        this.snackbars = this.snackbars.filter((sb) => !sb.isError)

      // add new snackbar
      this.snackbars.push({message, isError})

      // if not blocking, remove after timeout
      if (!isBlocking) {
        const idx = this.snackbars.findIndex((sb) => sb.message === message)
        setTimeout(() => {
          this.removeSnackbar(idx)
        }, 5000)
      }
    },
    removeSnackbar(index) {
      this.snackbars.splice(index, 1)
    },
    async fetchLatestData() {
      const url = getApiUrl(`/Offense/${this.data.recordId}`)
      const response = await authFetch(url, {
        method: 'GET'
      })

      if (response.ok) {
        const updatedData = await response.json()
        this.localData = { ...this.localData, ...updatedData }
        // this.addSnackbar('Daten wurden aktualisiert.', false)
      } else {
        // this.addSnackbar('Fehler beim Aktualisieren der Daten.', true)
      }
    }
  },
  computed: {
    formData() {
      return this.localData
    }
  }
}
</script>

<style scoped>
.snackbar {
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  background-color: #333;
  color: #fff;
  padding: 16px;
  border-radius: 4px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.3);
  z-index: 1000;
  width: fit-content;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.snackbar-message {
  flex-grow: 1;
  margin-right: 10px;
}

.snackbar-close {
  background: none;
  border: none;
  color: #fff;
  font-size: 20px;
  cursor: pointer;
}

.snackbar-close:hover {
  color: #ccc;
}

.error-message {
  background-color: red;
}

.success-message {
  background-color: rgb(63, 63, 63);
}

#background {
  height: 100%;
  width: 100%;
  z-index: -1;
}

#title {
  font-size: 30px;
  text-align: center;
}

#mode {
  color: rgb(222, 38, 38);
  text-align: center;
  margin-bottom: -0.5em;
}

.snackbar-container {
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 1000;
  display: flex;
  flex-direction: column;
}

.snackbar {
  margin-top: 10px;
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

</style>
