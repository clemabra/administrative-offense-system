/*
 * OPAS - Footer Component
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
 * along with this program.  If not, see https://www.gnu.org/licenses/.
 */

<template>
  <div id="app-footer">
    <ul id="list">
      <li>© OPAS 2024</li>
      <li><a>Hinweise</a></li>
      <li><a>Datenschutz</a></li>
      <li><a>Kontakt</a></li>
      <li><a>Barrierefreiheitserklärung</a></li>
      <li><a>Impressum</a></li>
      <router-link to="/styleguide" class="router-link">
        <li>
          <div id="style-icon"></div>
        </li>
      </router-link>
    </ul>
  </div>
</template>

<style scoped>
#app-footer {
  background: #404040;
  font-size: 20px;
  font-weight: bold;
  color: #fdfdfd;
  padding: 1% 6%;
  border-top: #E8C325 solid 9px;
}

#list {
  list-style: none;
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-wrap: wrap;
}

a {
  color: #fdfdfd;
  text-decoration: none;
  border-bottom: #fdfdfd solid 1px;
  cursor: pointer;
}

a:hover {
  border-bottom: rgba(253, 253, 253, 0) solid 1px;
}

#style-icon {
  background-image: url(@/assets/style-icon.svg);
  position: relative;
  height: 25px;
  width: 25px;
  float: right;
  cursor: pointer;
  transition: 0.1s;
}

#style-icon:hover {
  background-image: url(@/assets/style-icon-hovered.svg);
}

.router-link {
  border-bottom: rgba(253, 253, 253, 0) solid 1px;
}

@media only screen and (max-width: 480px) {
  #app-footer {
    font-size: 16px;
    border-top: #E8C325 solid 6px;
  }

  #list {
    list-style: none;
    display: block;
    justify-content: space-evenly;
    flex-wrap: wrap;
  }
}
</style>
<script setup lang="ts">
</script>