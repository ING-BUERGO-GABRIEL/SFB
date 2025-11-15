
<template>
  <v-locale-provider>
    <v-app :class="[]">
      <VerticalSidebarVue />
      <VerticalHeaderVue />

      <v-main
        class="page-wrapper"
        :aria-busy="isLoadingBody ? 'true' : 'false'"
      >
        <BodyLoaderWrapper />
        <v-container fluid class="content-wrapper">
          <div>
            <!-- Loader start -->
            <LoaderWrapper />
            <!-- Loader end -->
            <RouterView />
          </div>
        </v-container>
        <v-container fluid class="pt-0">
          <div>
            <FooterPanel />
          </div>
        </v-container>
      </v-main>
    </v-app>
  </v-locale-provider>
   
</template>

<script setup lang="ts">
import { RouterView } from 'vue-router';
import { storeToRefs } from 'pinia';
import { useUIStore } from '@/stores/ui';
import LoaderWrapper from './LoaderWrapper.vue';
import BodyLoaderWrapper from './BodyLoaderWrapper.vue';
import VerticalSidebarVue from './vertical-sidebar/VerticalSidebar.vue';
import VerticalHeaderVue from './vertical-header/VerticalHeader.vue';
import FooterPanel from './footer/FooterPanel.vue';

const uiStore = useUIStore();
const { isLoadingBody } = storeToRefs(uiStore);
</script>
<style>
.content-wrapper {
  position: relative;
  /* opcional: si quieres que ocupe toda la altura restante
     podrías darle height: calc(100vh - alturaDelNavYHeader) */
}

.page-wrapper {
  position: relative;
}
</style>
