<script setup lang="ts">
const props = defineProps({
  title: { type: String, default: '' },
  showHeader: { type: Boolean, default: true }
});
</script>

<template>
  <v-card variant="outlined" elevation="0" class="bg-surface">
    <v-card-item v-if="props.showHeader">
      <div class="d-sm-flex align-center justify-space-between">
        <v-card-title class="text-h6" style="line-height: 1.57">{{ props.title }}</v-card-title>
        <slot name="action"></slot>
      </div>
    </v-card-item>
    <v-divider></v-divider>
    <v-card-text>
      <slot></slot>
    </v-card-text>
    <v-card-actions>
      <slot name="footer"></slot>
    </v-card-actions>
  </v-card>
</template>
