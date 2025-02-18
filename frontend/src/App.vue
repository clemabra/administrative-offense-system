<template>
  <div id="app">
    <nav id="navbar-area">
      <AppNavbar :userName="userName" :isLoggedIn="isLoggedIn"/>
    </nav>
    
    <router-view id="main-content" @userLoggedIn="updateUsername" @userLogOut="logout"></router-view>
    
    <footer id="footer-area">
      <AppFooter/>
    </footer>
  </div>
</template>

<script setup>
import AppNavbar from '@/components/AppNavbar.vue'
import AppFooter from '@/components/AppFooter.vue'
</script>

<script>
export default {
  data() {
    return {
      userName: localStorage.getItem('username') || '',
      isLoggedIn: localStorage.getItem('token') ? true : false
    }
  },
  methods: {
    updateUsername(newUserName) {
      this.userName = newUserName
      localStorage.setItem('username', newUserName)
      this.isLoggedIn = true;
    },
    logout() {
      localStorage.removeItem('token')
      localStorage.removeItem('username')
      this.userName = ''
      this.isLoggedIn = false
    }
  }
}
</script>
  
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #0a1c2c;
  height: 100vh;
}

body {
  margin: 0;
  padding: 0;
  background: #fdfdfd;
}

#footer-area {
  bottom: 0px;
}

#main-content {
  min-height: 80vh;
}

.router-link {
  text-decoration: none;
  color: #0a1c2c;
}
</style>