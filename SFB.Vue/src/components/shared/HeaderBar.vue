<template>
  <v-row class="my-0" no-gutters>
    <v-col cols="12" class="pa-0">
      <v-card elevation="0">
        <v-card variant="outlined">
          <v-card-text :class="props.classCard">
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
const props = defineProps({
  classCard: { type: String, default: '' },
})

</script>
