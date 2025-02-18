<template>
  <div id="container">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background">
    <div class="menu-container">
      <div class="menu">
        <ul class="nav-list">
          <li>
            <router-link to="/ersterfassung" class="router-link">
              <div id="arrow-icon"></div>
              Ersterfassung
            </router-link>
          </li>
          <li>
            <router-link to="/falluebersicht" class="router-link">
              <div id="arrow-icon"></div>
              Fallübersicht
            </router-link>
          </li>
          <li>
            <router-link to="/weiserzeichen-konfiguration" class="router-link">
              <div id="arrow-icon"></div>
              Weiserzeichen konfigurieren
            </router-link>
          </li>
          <li>
            <router-link to="/meldungen-importieren" class="router-link">
              <div id="arrow-icon"></div>
              Meldungen importieren
            </router-link>
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<style scoped>
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

#background {
  height: 100%;
  width: 100%;
  z-index: -1;
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

.router-link {
  cursor: pointer;
  transition: 0.3s;
  text-decoration: none;
  color: inherit;
}

.router-link:hover #arrow-icon {
  background-image: url("@/assets/arrow-right-hovered.png");
}

.router-link:hover {
  color: #E8C325;
}
</style>
