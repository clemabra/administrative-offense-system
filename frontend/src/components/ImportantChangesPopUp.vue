/*
 * OPAS - Important Changes Popup Component
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
<div v-if="showModal" class="custom-modal">
  <div class="modal-content">
    <h3>Wichtige Änderungen</h3>
    <p class="modeal-message">{{ modalMessage }}</p>
    <div class="modal-buttons">
      <button @click="confirm(true)" class="modal-button confirm-button">Ja</button>
      <button @click="confirm(false)" class="modal-button cancel-button">Nein</button>
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
    async showWarningConfirmation(message) {
      this.modalMessage = message
      this.showModal = true
      
      // return promise that resolves when user clicks a button
      return new Promise((resolve) => {
        this.resolveConfirm = resolve
      })
    },
    confirm(result) {
      this.showModal = false
      this.resolveConfirm(result)
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