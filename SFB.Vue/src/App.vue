<template>
  <RouterView />
  <MessageDialog />
</template>

<script setup>
import { RouterView } from 'vue-router'
import MessageDialog from '@/components/shared/MessageDialog.vue'
</script>
