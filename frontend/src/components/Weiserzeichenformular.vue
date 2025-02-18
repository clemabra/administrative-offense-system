<template>
  <div id="input-form-container">
    <div id="input-form">
      <form @submit.prevent="handleSubmit">
        <div class="table-container">
          <table id="formal-table">

            <tr>
              <th>Ez.</th>
              <th></th>
              <th>Wz.</th>
            </tr>

            <tr v-for="(item, index) in firstHalf" :key="'first-' + index">
              <td>
                <input
                  type="text"
                  v-model="item.fallnummer"
                  placeholder="Ez."
                  readonly
                  tabindex="-1"
                  class="readonly-field"
                  style="width: 1.7rem"
                />
              </td>

              <td style="text-align: center; align-content: center; vertical-align: middle;">
                >
              </td>

              <td>
                <input
                  type="text"
                  v-model="item.weiserzeichen"
                  placeholder="Wz."
                  maxlength="2"
                  style="width: 2.7rem"
                />
              </td>
            </tr>
          </table>
          <table id="formal-table">

            <tr>
              <th>Ez.</th>
              <th></th>
              <th>Wz.</th>
            </tr>

            <tr v-for="(item, index) in secondHalf" :key="'second-' + index">
              <td>
                <input
                  type="text"
                  v-model="item.fallnummer"
                  placeholder="Ez."
                  readonly
                  tabindex="-1"
                  class="readonly-field"
                  style="width: 1.7rem"
                />
              </td>

              <td style="text-align: center; align-content: center; vertical-align: middle;">
                >
              </td>

              <td>
                <input
                  type="text"
                  v-model="item.weiserzeichen"
                  placeholder="Wz."
                  maxlength="2"
                  style="width: 2.7rem"
                />
              </td>
            </tr>
          </table>
        </div>

        <div id="submit-and-response">
          <button id="submit-button">Speichern</button>
        </div>

      </form>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    initialData: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      formData: []
    }
  },
  computed: {
    firstHalf() {
      return this.formData.slice(0, 5)
    },
    secondHalf() {
      return this.formData.slice(5)
    }
  },
  watch: {
    initialData: {
      immediate: true,
      handler(newData) {
        if (Array.isArray(newData)) {
          this.formData = newData.map(item => ({
            id: item.id,
            rowVersion: item.rowVersion || '',
            fallnummer: item.fallnummer || '',
            weiserzeichen: item.weiserzeichen || ''
          }))
        } else {
          this.formData = []
        }
      }
    }
  },
  methods: {
    handleSubmit() {

      // Filter out empty input fields
      const nonEmptyFields = this.formData.filter(item => item.weiserzeichen.trim() !== "")

      // Check if all non-empty fields contain only numeric values
      const allNumeric = nonEmptyFields.every(item => /^[0-9]+$/.test(item.weiserzeichen))
      if (!allNumeric) {
        this.responseMessage = 'Weiserzeichen dürfen nur Zahlen enthalten.'
        this.$emit('responseMessage', { message: this.responseMessage, type: 'error' })
        return
      }

      // Check if all non-empty fields have unique values
      const uniqueWeiserzeichen = new Set(nonEmptyFields.map(item => item.weiserzeichen))
      if (uniqueWeiserzeichen.size !== nonEmptyFields.length) {
        this.responseMessage = 'Weiserzeichen können nicht mehrfach vergeben werden.'
        this.$emit('responseMessage', { message: this.responseMessage, type: 'error' })
        return
      }

      // Check if any non-empty field has the value "0"
      const isZero = nonEmptyFields.some(item => item.weiserzeichen === "0")
      if (isZero) {
        this.responseMessage = 'Weiserzeichen können nicht Null sein.'
        this.$emit('responseMessage', { message: this.responseMessage, type: 'error' })
        return
      }

      this.$emit('update', this.formData)
      this.responseMessage = 'Daten wurden erfolgreich gespeichert.'
      this.$emit('responseMessage', { message: this.responseMessage, type: 'success' })
    }
  }
}
</script>

<style scoped>
#input-form-container {
  background: #fdfdfd;
  width: 100%;
}

#input-form {
  padding: 1% 6%;
  font-size: 20px;
  font-weight: bold;
  margin: 0;
  display: flex;
  justify-content: center;
}

input {
  border: solid 1px black;
  box-sizing: border-box;
  outline: 0;
  position: relative;
  width: 100%;
  font-family: Arial, serif;
}

input[type="text"] {
  padding: .45rem;
  font-size: 15px;
  font-family: Arial, serif;
}

.table-container {
  display: flex;
  justify-content: space-between;
  gap: 2rem;
}

table {
  margin-bottom: 18px;
  table-layout: fixed;
  flex: 1;
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

.readonly-field {
  background: #f1f1f1;
  color: gray;
  font-family: Arial, Helvetica, sans-serif;
  font-weight: normal;
}

#submit-and-response {
  text-align: center;
  padding-top: 6%;
}

#submit-button {
  background: #404040;
  font-size: 20px;
  padding: 3% 9%;
  color: #fdfdfd;
  border: none;
  cursor: pointer;
  transition: 0.3s;
  margin: 0 1%;
}

#submit-button:hover:enabled {
  background: #E8C325;
}
</style>
