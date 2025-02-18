<template>
  <div class="container">
    <img style="height: 100%; width: 100%; z-index: -1;" src="@/assets/landesverwaltungsamt.jpeg" alt="background">

    <div class="snackbar-container">
      <div v-if="responseMessage" :class="['snackbar', responseType]">
        <span class="snackbar-message"> {{ responseMessage }} </span>
        <button class="snackbar-close" @click="responseMessage = ''">&times;</button>
      </div>
    </div>

    <router-link to="/ordnungswidrigkeiten-verwalten" class="back-arrow">
      <i class="fas fa-arrow-left"></i>
    </router-link>

    <h1 style="font-size: 30px; text-align: center; margin-bottom: 0;">Weiserzeichenregelung</h1>
    <p style="font-size: 14px; text-align: center; margin-bottom: 0;">Aktenzeichen-Endziffer (Ez.) > Weiserzeichen (Wz.)</p>

    <div v-if="items.length">
      <Weiserzeichenformular :initialData="items" @update="updateData" @responseMessage="updateMessage"/>
    </div>
    <div v-else>
      <p style="font-size: 20px; font-weight: bold; text-align: center; margin-top: 20px;">
        Daten konnten nicht geladen werden.
      </p>
    </div>
  </div>
</template>

<script>
import Weiserzeichenformular from "@/components/Weiserzeichenformular.vue"
import { getApiUrl } from "@/api"
import { authFetch } from "@/utils/authFetch"

export default {
  components: { Weiserzeichenformular },
  data() {
    return {
      items: [],
      responseMessage: '',
      responseType: '',
    }
  },
  methods: {
    async fetchItems() {
      try {
        const response = await authFetch(getApiUrl('/Wz'))
        const result = await response.json()
        this.items = Array.isArray(result.data) ? result.data : []
      } catch (error) {
        console.error('Error fetching items:', error)
        this.items = []
      }
    },
    async updateData(updatedItems) {
      try {
        for (let item of updatedItems) {
          const {id, rowVersion, fallnummer, weiserzeichen} = item
          const response = await authFetch(getApiUrl(`/Wz/${id}`), {
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify({
              id,
              rowVersion,
              fallnummer,
              weiserzeichen
            })
          })

          if (!response.ok) {
            if (response.status === 409) {
              this.updateMessage({
                message: 'Konfliktfehler: Die Daten wurden inzwischen geändert. Bitte laden Sie die Seite neu.',
                type: 'error'
              })
              return
            } else {
              const errorText = await response.text()
              console.error(`Failed to update data for item with id ${id}:`, errorText)
              return
            }
          }

          const result = await response.json()
          if (result.message) {
            this.updateMessage({message: result.message, type: 'success'})
          }
          item.rowVersion++
        }
        this.items = updatedItems
      } catch (error) {
        console.error('Error updating data:', error)
      }
    },
    updateMessage({message, type}) {
      this.responseMessage = message
      this.responseType = type

      if (type === 'success') {
        setTimeout(() => {
          this.responseMessage = ''
          this.responseType = ''
        }, 3000)
      }
    }
  },
  mounted() {
    this.fetchItems()
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
  margin-top: 10px;
}

.snackbar.success {
  background-color: #333;
}

.snackbar.error {
  background-color: #FF0000FF;
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
