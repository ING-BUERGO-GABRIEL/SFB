import { moduleService } from '@/services/cms/moduleService'
import { productService } from '@/services/iaw/productService'
import { warehouseService } from '@/services/iaw/warehouseService'
import { presentationService } from '@/services/iaw/presentationService'
import { inventoryTxnService } from '@/services/iaw/inventoryTxnService'
import { supplierService } from '@/services/pcm/supplierService'
import { purchaseService } from '@/services/pcm/purchaseService'
import { salesTxnService } from '@/services/som/salesTxn'
import { useUIStore } from '@/stores/ui'

export const configServices = {
    install: (app) => {

        const moduleServ = moduleService();
        const productServ = productService();
        const warehouseServ = warehouseService();
        const presentationServ = presentationService();
        const invTxnServ = inventoryTxnService();
        const supplierServ = supplierService();
        const purchaseServ = purchaseService();
        const salesServ = salesTxnService();
        const uiStore = useUIStore()

        app.provide('services', {
            moduleServ,
            productServ,
            warehouseServ,
            presentationServ,
            invTxnServ,
            supplierServ,
            purchaseServ,
            salesServ,
            uiStore
        });
    },
};
