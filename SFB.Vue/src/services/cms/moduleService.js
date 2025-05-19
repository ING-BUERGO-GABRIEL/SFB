import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import * as AntIcons from '@ant-design/icons-vue';

const DefaultIcon = AntIcons['QuestionOutlined'];
const baseController = 'api/CMS/Module/';
const getRoute = method => `${baseController}${method}`;

const mapModulesToSidebar = modules => {
  const result = [];

  for (const mod of modules) {
    result.push({ header: mod.Name }); // encabezado tipo 'Navigation', 'Authentication'

    for (const opt of mod.OptionMenus) {
      result.push({
        title: opt.Name,
        icon: AntIcons[opt.Icon] || DefaultIcon, // puedes mapear opt.Icon si gustas
        to: opt.Route
      });
    }
  }

  return result;
}

export const moduleService = defineStore('moduleService', {
  state: () => ({
    options: []
  }),
  actions: {
    async getOptionsMenu() {
      try {
        const route = getRoute('GetHomeModules');
        const apiResult = await apiClient.get(route);
        this.options = mapModulesToSidebar(apiResult.Data);
        return this.options;
      } catch (error) {
        console.error('Error al obtener módulos:', error);
        throw error;
      }
    }
  }
});
