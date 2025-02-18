<template>
  <div id="import-container">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background" />

    <router-link to="/ordnungswidrigkeiten-verwalten" class="back-arrow">
      <i class="fas fa-arrow-left"></i>
    </router-link>

    <h1 id="title">
      Import für Meldungen <br> 
      Ordnungswidrigkeit nach §121 Abs.1 Nr.6 SGB XI
    </h1>
    <h2 id="mode">* CSV-Datei hochladen *</h2>

    <div class="csv-upload">
      <div class="upload-container">
        <input
          type="file"
          ref="fileInput"
          accept=".csv"
          @change="handleFileUpload"
          class="file-input"
        />
        <button 
          @click="uploadFile" 
          :disabled="!selectedFile" 
          class="upload-button"
        >
          CSV-Datei hochladen
        </button>
      </div>
      
      <div v-if="message" :class="['message', messageType]">
        {{ message.split('\n').join(' - ') }}
      </div>
    </div>
  </div>
</template>

<script>
import { getApiUrl } from '../api'
import { authFetch } from '../utils/authFetch'

export default {
  data() {
    return {
      selectedFile: null,
      message: '',
      messageType: 'info',
      isSubmitting: false
    }
  },
  methods: {
    handleFileUpload(event) {
      this.selectedFile = event.target.files[0]
      this.message = ''
    },
    async uploadFile() {
      if (!this.selectedFile || !this.selectedFile.name || this.isSubmitting) return
      this.isSubmitting = true

      try {
        if (!this.selectedFile.name.endsWith('.csv') && !this.selectedFile.name.endsWith('.CSV')) {
          this.message = 'Unzulässiger Dateityp. Bitte laden Sie eine CSV-Datei hoch.'
          this.messageType = 'error'
          return
        }

        const validMimeTypes = ['text/csv', 'application/vnd.ms-excel']
        if (!validMimeTypes.includes(this.selectedFile.type)) {
          this.message = 'Unzulässiger Dateityp. Bitte laden Sie eine CSV-Datei hoch.'
          this.messageType = 'error'
          this.selectedFile = null
          this.$refs.fileInput.value = ''
          return
        }

        if (this.selectedFile.size === 0) {
          this.message = 'Die Datei ist leer.'
          this.messageType = 'error'
          return
        }
        
        const MAX_SIZE = 2 * 1024 * 1024 // 2 MB

        if (this.selectedFile.size > MAX_SIZE) {
          this.message = 'Die Datei ist zu groß. Bitte laden Sie eine Datei kleiner als 2MB hoch.'
          this.messageType = 'error'
          return
        }

        const formData = new FormData()
        formData.append('file', this.selectedFile)
        
        const response = await authFetch(getApiUrl('/Offense/import-csv'), {
          method: 'POST',
          body: formData
        })

        const responseText = await response.text()
        let result = null

        try {
          result = JSON.parse(responseText)
        } catch (error) {
          console.error('Fehlerhafte Backend-Antwort: ', responseText)
          throw new Error('Unerwartete Antwort vom Server.')
        }
        
        if (!response.ok) {
          console.log('API error:', result)
          if (result.details) {
            this.message = `${result.message}\n${result.details}`
          } else {
            this.message = result.message || 'Fehler beim Hochladen der Datei.'
          }
          this.messageType = 'error'
          this.selectedFile = null
          this.$refs.fileInput.value = ''
          return
        }

        this.message = result.message || 'CSV-Datei erfolgreich hochgeladen.'
        this.messageType = 'success'
        this.selectedFile = null
        this.$refs.fileInput.value = ''
      } catch (error) {
        console.error('API error:', error)
        this.message = 'Fehler beim Hochladen der Datei:' + error
        this.messageType = 'error'
        this.selectedFile = null
        this.$refs.fileInput.value = ''
      } finally {
        this.isSubmitting = false
      }
    }
  }
}
</script>

<style scoped>
#import-container {
  width: 100%;
  height: auto;
}

#background {
  width: 100%;
  height: auto;
  z-index: -1;
  display: block;
}

#title {
  font-size: 30px;
  text-align: center;
  margin-top: 1em;
}

#mode {
  color: rgb(222, 38, 38);
  text-align: center;
  margin-bottom: 1em;
}

.csv-upload {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  font-size: 16px;
}

.upload-container {
  padding: 20px;
  border: 2px dashed #404040;
  border-radius: 4px;
  text-align: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 2em;
}

.upload-button {
  padding: 8px 16px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.upload-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.message {
  padding: 10px;
  margin-top: 10px;
  border-radius: 4px;
  font-size: 0.9rem;
  white-space: pre-line;
}

.success {
  background-color: #dff0d8;
  color: #3c763d;
}

.error {
  background-color: #f2dede;
  color: #a94442;
}

.info {
  background-color: #d9edf7;
  color: #31708f;
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
