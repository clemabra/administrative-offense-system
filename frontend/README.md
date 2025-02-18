# src

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
# OPAS UI
Design template @figma: https://www.figma.com/design/UkqFSVSTzatC9xJiE6rcDp/OPUS?node-id=126-154&t=IngUoR9Sc5Zerk75-0
## Using VueJS

# Notes


```
<!-- src/components/DataSender.vue -->
<template>
  <div>
    <h1>Send Data to .NET Core API</h1>
    <form @submit.prevent="sendData">
      <input v-model="data.name" placeholder="Name" />
      <input v-model="data.value" placeholder="Value" />
      <button type="submit">Send</button>
    </form>
    <p>{{ responseMessage }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      data: {
        name: '',
        value: ''
      },
      responseMessage: ''
    };
  },
  methods: {
    async sendData() {
      try {
        const response = await axios.post('http://localhost:5050/api/data', this.data);
        this.responseMessage = response.data.message;
      } catch (error) {
        this.responseMessage = 'Error: ' + error.response.data;
      }
    }
  }
};
</script>

```