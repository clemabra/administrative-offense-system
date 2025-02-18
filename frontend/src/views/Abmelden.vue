<template>
  <div id="container">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background">
    <h1 id="title">Abmeldung</h1>
      
    <ul class="nav-list">
      <li>
        <button
          id="cancel-button"
          class="nav-list-item"
          type="button"
          @click="handleCancel"
          aria-label="cancel-button"
        >
          <div id="arrow-icon"></div>
          Zurück
        </button>
      </li>
      <li>
        <button
          id="logout-button"
          class="nav-list-item"
          aria-label="logout-button"
          :disabled="!isLoggedIn"
          @click="handleLogout"
        >
          <div id="arrow-icon"></div>
          Abmelden
        </button>	
      </li>
    </ul>
  </div>
</template>

<script>
export default {
	methods: {
		handleCancel() {
			window.history.back()
		},
    handleLogout() {
      this.$emit('userLogOut')
      this.$router.push('/')
    },
	},
	computed: {
		isLoggedIn() {
      return !!localStorage.getItem('token')
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
  font-size: 32px;
  text-align: center;
  margin: 20px 0;
}

.nav-list {
  list-style: none;
  padding: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.nav-list-item {
  cursor: pointer;
  font-size: 20px;
  font-weight: bold;
  color: #404040;
  background: none;
  border: none;
  padding: 10px;
  display: flex;
  align-items: center;
  transition: 0.3s;
}

.nav-list-item:disabled {
  cursor: not-allowed;
  color: #aaa;
}

.nav-list-item:hover {
  color: #E8C325;
}

#arrow-icon {
  background-image: url("@/assets/arrow-right.png");
  display: inline-block;
  width: 24px;
  height: 24px;
  background-size: contain;
  background-repeat: no-repeat;
  margin-right: 10px;
  transition: 0.3s;
}

.nav-list-item:hover #arrow-icon {
  background-image: url("@/assets/arrow-right-hovered.png");
}

#response {
  color: red;
  text-align: center;
  margin-top: 20px;
}
</style>