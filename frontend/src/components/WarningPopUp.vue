<template>
<div v-if="showModal" class="custom-modal">
  <div class="modal-content">
    <h3>Warnung</h3>
    <p class="modeal-message">{{ modalMessage }}</p>
    <div class="modal-buttons">
      <button @click="confirm" class="modal-button confirm-button">Schließen</button>
    </div>
  </div>
</div>
</template>

<script>
export default {
  data() {
    return {
      showModal: false,
      modalMessage: '',
      resolveConfirm: null
    }
  },
  methods: {
    async showWarning(message) {
      this.showModal = true
      this.modalMessage = message
      
      // return promise that resolves when user clicks a button
      return new Promise((resolve) => {
        this.resolveConfirm = resolve
      })
    },
    confirm() {
      this.showModal = false
      this.resolveConfirm(true)
    }
  }
}
</script>

<style scoped>
.custom-modal {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding-top: 10%;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px 30px;
  border-radius: 10px;
  width: 400px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

h3 {
  margin-top: 0;
  font-size: 1.5rem;
  color: #000000;
}

.modal-message {
  margin: 20px 0;
  font-size: 0.7rem;
  text-align: left;
  color: #000000;
}

.modal-buttons {
  display: flex;
  justify-content: center;
  gap: 15px;
  margin-top: 20px;
}

.modal-button {
  padding: 10px 25px;
  font-size: 1rem;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.confirm-button {
  background-color: #4caf50;
  color: white;
}

.confirm-button:hover {
  background-color: #45a049;
}

.cancel-button {
  background-color: #f44336;
  color: white;
}

.cancel-button:hover {
  background-color: #e53935;
}
</style>