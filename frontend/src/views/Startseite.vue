<template>
  <div id="container">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background">
    <div class="menu-container">
      <div class="menu">
        <h1 id="title">OWiG121</h1>
        <p id="description">Ordnungswidrigkeitsverfahren<br/>nach §121 Abs.1 Nr.6 SGB XI - Soziale Pflegeversicherung
        </p>
        <router-link to="/ordnungswidrigkeiten-verwalten" class="router-link">
          <button id="btn">Ordnungswidrigkeiten verwalten</button>
        </router-link>
        <ul class="nav-list">
          <!--
          <li>
            <router-link to="/anmelden" class="router-link">
              <a id="nav-list-item">
                <div id="arrow-icon"></div>
                Anmelden
              </a>
            </router-link>
          </li>
          -->
          <li>
            <a id="nav-list-item">
              <div id="arrow-icon"></div>
              Häufige Fragen
            </a>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style scoped>
#background {
  height: 100%;
  width: 100%;
  z-index: -1;
}

.menu-container {
  background: #fdfdfd;
  width: 100%;
  min-height: 65vh;
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

#arrow-icon {
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

#nav-list-item {
  cursor: pointer;
  transition: 0.3s;
}

#nav-list-item:hover #arrow-icon {
  background-image: url("@/assets/arrow-right-hovered.png");
}

#nav-list-item:hover {
  color: #E8C325;
}

.router-link {
  text-decoration: none;
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