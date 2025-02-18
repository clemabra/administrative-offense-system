<template>
  <div id="navbar">
    <router-link to="/" class="router-link">
      <img id="logo" src="../assets/logo.png" alt="logo">
    </router-link>

    <router-link to="/Anmelden" v-if="!isLoggedIn" class="router-link">
      <div class="account-container">
        <img id="account" src="../assets/account.png" alt="account"/>
        <p style="margin: 2px">Anmelden</p>
      </div>
    </router-link>

    <router-link to="/Abmelden" v-else-if="isLoggedIn" class="router-link">
      <div class="account-container">
        <img id="account" src="../assets/account.png" alt="account"/>
        <p v-if="userName" style="margin: 2px">Angemeldet als {{ userName }}</p>
      </div>
    </router-link>
  </div>
</template>

<script>
export default {
  props: {
    userName: {
      type: String,
      required: true
    },
    isLoggedIn: {
      type: Boolean,
      required: true
    }
  }
}
</script>

<style scoped>
#navbar {
  position: sticky;
  background: #fdfdfd;
  padding: 1% 6%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 20px;
  font-weight: bold;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

#logo {
  height: 80px;
  width: auto;
  cursor: pointer;
}

.account-container {
  display: flex;
  align-items: center;
  border-bottom: rgba(232, 195, 37, 0) solid 1px;
  cursor: pointer;
  transition: 0.3s;
}

.account-container:hover {
  border-bottom: #E8C325 solid 1px;
}

.router-link {
  transition: 0.3s;
}

.router-link:hover {
  transform: scale(1.05);
}

#account {
  height: 26px;
  width: auto;
  position: relative;
  bottom: 2px;
  right: 2px;
  margin: 0;
  padding: 0;
}

@media only screen and (max-width: 480px) {
  .navbar {
    font-size: 16px;
  }

  #logo {
    height: 60px;
  }

  #account {
    height: 22px;
  }
}
</style>