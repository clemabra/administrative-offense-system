<template>
  <div class="case-overview">
    <img id="background" src="../assets/landesverwaltungsamt.jpeg" alt="background">

    <router-link to="/ordnungswidrigkeiten-verwalten" class="back-arrow">
      <i class="fas fa-arrow-left"></i>
    </router-link>

    <div class="menu-container">
      <div class="menu">

        <h1>Fallübersicht</h1>

        <div id="search-area">
          <i class="fas fa-search"></i>
          <input type="text" placeholder="Suche nach Fallnummer oder Name" @input="handleSearchString"/>
        </div>

        <div v-if="visibleItems.length">
          <div v-for="item in visibleItems" :key="item.recordId" class="item" @click="openEditView(item)">
            <div id="info">
              <p><strong>Aktenzeichen:</strong> 504.1.{{item.weiserzeichen}}/{{ item.fallnummer }}</p>
              <p><strong>Name:</strong> {{ item.titel }} {{ item.nachname }}, {{ item.vorname }}</p>
            </div>
            <div id="edit-icon">
              <i class="fas fa-edit"></i>
            </div>
          </div>
        </div>

        <div v-else>
          <p>Es wurden keine Fälle gefunden.</p>
        </div>

        <div id="back-to-top" v-if="visibleItems.length > 10" @click="scrollToTop">
          <a href="#background" >
            <i class="fas fa-arrow-up"></i>
            Zurück nach oben
          </a>
        </div>
        
      </div>
    </div>
  </div>
</template>

<script setup>
import { getApiUrl } from '@/api'
import { authFetch } from '@/utils/authFetch'
</script>

<script>
export default {
  data() {
    return {
      items: [],
      searchString: '',
      visibleItems: []
    }
  },
  methods: {
    async openEditView(item) {
      try {
        const response = await fetch(getApiUrl(`/Offense/${item.recordId}`))
        const result = await response.json()

        this.$router.push({
          path: `/bearbeiten/${item.recordId}`,
          query: {data: JSON.stringify(result.data)}
        })
      } catch (error) {
        console.error('Error fetching item:', error)
      }
    },
    async fetchItems() {
      try {
        const response = await authFetch(getApiUrl('/Offense'))
        const result = await response.json()
        this.items = result.data
        // console.log('Items:', this.items)
        this.updateVisibleItems()
      } catch (error) {
        console.error('Error fetching items:', error)
      }
    },
    updateVisibleItems() {
      const searchLower = this.searchString.toLowerCase()
      this.visibleItems = this.items.filter(item => {
      return item.fallnummer.toLowerCase().includes(searchLower) || 
           item.nachname.toLowerCase().includes(searchLower) || 
           item.vorname.toLowerCase().includes(searchLower)
      })
    },
    handleSearchString(event) {
      // console.log('Search string:', event.target.value)
      this.searchString = event.target.value
      this.updateVisibleItems()
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      })
    }
  },
  mounted() {
    this.fetchItems()
  }
}
</script>

<style scoped>
#background {
  height: 100%;
  width: 100%;
  z-index: -1;
}

.menu-container {
  background: #fdfdfd;
  width: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}

.menu {
  padding: 1% 6%;
  font-size: 20px;
  font-weight: bold;
  max-width: 800px;
  width: 100%;
}

#search-area {
  display: flex;
  align-items: center;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 0;
  padding: 10px 20px;
  margin-bottom: 20px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

#search-area i {
  color: #aaa;
  margin-right: 10px;
}

#search-area input {
  border: none;
  outline: none;
  width: 100%;
  font-size: 16px;
  padding: 5px 0;
}

.menu .item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border: 1px solid #ddd;
  padding: 20px;
  margin-bottom: 20px;
  background: #fff;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  transition: 0.3s;
  border-radius: 0;
  position: relative;
  overflow: hidden;
}

.menu .item:hover {
  background: #E8C325;
  transform: scale(1.01);
  cursor: pointer;
}

.menu .item p {
  margin: 5px 0;
  font-size: 16px;
  cursor: pointer;
}

.menu .item p strong {
  color: #333;
}

.edit-icon {
  background: #0a1c2c;
  color: #fff;
  padding: 10px;
  border-radius: 50%;
  transition: right 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  right: -40px;
}

.menu .item:hover .edit-icon {
  right: 0;
}

.tooltip-text {
  visibility: hidden;
  width: 120px;
  background-color: #0a1c2c;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  font-size: 15px;
  font-weight: bold;
  padding: 5px 5px;
  position: absolute;
  z-index: 1;
  bottom: 125%;
  left: 50%;
  margin-left: -60px;
  opacity: 0;
  transition: opacity 0.3s;
}

.edit-icon:hover .tooltip-text {
  visibility: visible;
  opacity: 1;
}

.edit-icon i {
  font-size: 16px;
}

#back-to-top {
  color: #fff;
  padding: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: 0.3s;
  text-decoration: none;
}

#back-to-top i {
  margin-right: 5px;
}

#back-to-top:hover {
  scale: 1.1;
}

#back-to-top a {
  text-decoration: none;
  color: #000000;
  transition: 0.3s;
}

#back-to-top a:hover {
  color: #E8C325;
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