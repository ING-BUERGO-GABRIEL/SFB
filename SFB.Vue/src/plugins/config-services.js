import { moduleService } from '@/services/cms/moduleService'
import { productService } from '@/services/iaw/productService'
import { warehouseService } from '@/services/iaw/warehouseService'
import { presentationService } from '@/services/iaw/presentationService'
import { inventoryTxnService } from '@/services/iaw/inventoryTxnService'
import { useUIStore } from '@/stores/ui'

export const configServices = {
    install: (app) => {

        const moduleServ = moduleService();
        const productServ = productService();
        const warehouseServ = warehouseService();
        const presentationServ = presentationService();
        const invTxnServ = inventoryTxnService();
        const uiStore = useUIStore()

        app.provide('services', {
            moduleServ,
            productServ,
            warehouseServ,
            presentationServ,
            invTxnServ,
            uiStore
        });
    },
};
