<template>
  <v-navigation-drawer left v-model="customizer.Sidebar_drawer" elevation="0" rail-width="60" mobile-breakpoint="lg" app
    class="leftSidebar" :rail="customizer.mini_sidebar" expand-on-hover>
    <div class="pa-5">
      <Logo />
    </div>

    <perfect-scrollbar class="scrollnavbar">
      <v-list aria-busy="true" aria-label="menu list">
        <template v-for="(item, i) in sidebarMenu" :key="i">
          <NavGroup :item="item" v-if="item.header" :key="item.title" />
          <v-divider class="my-3" v-else-if="item.divider" />
          <NavCollapse class="leftPadding" :item="item" :level="0" v-else-if="item.children" />
          <NavItem :item="item" v-else />
        </template>
      </v-list>
    </perfect-scrollbar>
  </v-navigation-drawer>
</template>

<script setup>
import { shallowRef , onMounted } from 'vue';
import { useCustomizerStore } from '../../../stores/customizer';
import { moduleService } from '@/services/cms/moduleService'
import sidebarItems from './sidebarItem';

import NavGroup from './NavGroup/NavGroup.vue';
import NavItem from './NavItem/NavItem.vue';
import NavCollapse from './NavCollapse/NavCollapse.vue';
import Logo from '../logo/LogoDark.vue';

const customizer = useCustomizerStore();
const sidebarMenu = shallowRef([]);
const module = moduleService();

onMounted(async () => {
  let listOptions = await module.getOptionsMenu();
  sidebarMenu.value = import.meta.env.MODE === 'development'
    ? [...listOptions, ...sidebarItems]
    : [...listOptions];
})

</script>
