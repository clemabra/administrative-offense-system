<template>
  <div id="container">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background">
    <div class="input-form-container">
      <h1 id="title">Anmeldung</h1>
      <p id="description">Bitte melden Sie sich an, um die Anwendung zu nutzen.</p>

			<div id="input-form">
				<form @submit.prevent="handleSubmit">
					<table id="login-table">
						<td>
							<tr>
								<label for="username" class="field-label">Benutzername oder E-Mail</label>
								<input
									type="text"
									id="username"
                  v-model="username"
									required
									@focusout="validateLogin()"
									placeholder="Benutzername/E-Mail"
								>
							</tr>
							<tr>
								<label for="password" class="field-label">Passwort</label>
								<input
									type="password"
									id="password"
                  v-model="password"
									required
									@focusout="validateLogin()"
									placeholder="Passwort"
								>
							</tr>
						</td>
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
						id="submit-button"
						aria-label="send-login-info"
            :disabled="!username || !password || isSubmitting"
            :style="{ cursor: !username || !password || isSubmitting ? 'not-allowed' : 'pointer' }"
            @click="handleSubmit"
          >Anmelden
					</button>
          <p v-if="hasErrors" id="response">{{ responseMessage }}</p>
        </div>
				</form>
			</div>
      
    </div>
  </div>
</template>

<script setup>
import { getApiUrl } from '../api'
</script>

<script>
export default {
	data() {
		return {
			username: '',
      password: '',
      responseMessage: '',
      isLoggedIn: false,
      hasErrors: false,
      isSubmitting: false,
		}
	},
	methods: {
		async handleSubmit() {
      if (this.isSubmitting) return
      this.isSubmitting = true

      this.validateLogin()
      if (this.responseMessage != '') {
        this.isSubmitting = false
        return
      }

      const loginData = {
        username: this.username,
        password: this.password
      }

      try {
        const response = await fetch(getApiUrl('/Authentication/login'), {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json'
          },
          body: JSON.stringify(loginData)
        })

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`)
        }

        const result = await response.json()
        this.responseMessage = result.message

        if (result.token) {
          localStorage.setItem('token', result.token)
          localStorage.setItem('username', this.username)
          this.$emit('userLoggedIn', this.username)
        }
        
        this.isLoggedIn = true
        
        this.$router.push('/')
        return result
      } catch (error) {
        alert('Error: ' + error.message)
      } finally {
        this.isSubmitting = false
      }
		},
		handleCancel() {
			// clear input fields
			this.username = ''
			this.password = ''
			window.history.back()
		},
		validateLogin() {
      if (this.username === '' || this.password === '')
        this.responseMessage = 'Bitte geben Sie Benutzername/E-Mail und Passwort ein'
      else this.responseMessage = ''
		}
	},
	computed: {
		isLoggedIn() {
			// check if user is logged in
			// if user is logged in, set responseMessage to 'Anmeldung erfolgreich'
			// if user is not logged in, set responseMessage to 'Anmeldung fehlgeschlagen'
		},
		hasErrors() {
			// check if responseMessage is not empty
			// if responseMessage is not empty, return true
			// if responseMessage is empty, return false
		}
	},
	created() {
		/*
		const token = localStorage.getItem('token')
		if (token) {
			const payload = JSON.parse(atob(token.split('.')[1]))
			this.userName = payload.sub
		} 
		*/
	}
}
</script>

<style scoped>
#input-form {
  font-size: 20px;
  font-weight: bold;
  margin: 0;
  display: flex;
  justify-content: left;
}

#background {
  height: 100%;
  width: 100%;
  z-index: -1;
}

.input-form-container {
  background: #fdfdfd;
  padding: 0 5%;
}

.menu {
  padding: 1% 6%;
  font-size: 20px;
  font-weight: bold;
}

#btn {
  background: #404040;
  font-size: 20px;
  padding: 1% 3%;
  color: #fdfdfd;
  border: none;
  cursor: pointer;
  transition: 0.3s;
}

#btn:hover {
  background: #E8C325;
}

.nav-list {
  list-style: none;
  padding: 0;
}

.arrow-icon {
  background-image: url("@/assets/arrow-right.png");
  display: inline-block;
  width: 24px;
  height: 24px;
  background-size: contain;
  background-repeat: no-repeat;
  position: relative;
  top: 5px;
  transition: 0.3s;
}

.nav-list-item {
  cursor: pointer;
  transition: 0.3s;
}

.nav-list-item:hover .arrow-icon {
  background-image: url("@/assets/arrow-right-hovered.png");
}

.nav-list-item:hover {
  color: #E8C325;
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

.field-label {
  font-size: 14px;
  margin-bottom: 15px;
  font-family: Arial, Helvetica, sans-serif;
}

.error-container {
  width: 100%;
  margin-top: 5px;
}

.error-message {
  color: red;
  font-size: 12px;
  word-wrap: break-word;
  display: block;
}

.error-border {
  border-color: red !important;
}

#submit-and-response {
	display: flex;
	justify-content: left;
	margin: 20px 0 20px 0;
}

#submit-button, #cancel-button {
  background: #404040;
  font-size: 17px;
  padding: 2% 5%;
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

@media only screen and (max-width: 480px) {
  #title {
    font-size: 32px;
  }

  #description {
    font-size: 20px;
  }

  #btn {
    font-size: 16px;
    padding: 4% 8%;
  }
}
</style>