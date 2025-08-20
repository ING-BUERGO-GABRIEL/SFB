<template>
  <v-row class="my-0">
    <v-col cols="12">
      <v-card elevation="0">
        <v-card variant="outlined">
          <v-card-text>
            <div class="d-flex align-center">
              <slot />
            </div>
          </v-card-text>
        </v-card>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup>
// si luego quieres props, los defines aquí con defineProps()
// por ahora no necesitas nada más
</script>

<style scoped>
/* si quieres estilos extra, añádelos aquí */
</style>
