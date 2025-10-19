import { moduleService } from '@/services/cms/moduleService'
import { productService } from '@/services/iaw/productService'
import { warehouseService } from '@/services/iaw/warehouseService'
import { inventoryTxnService } from '@/services/iaw/inventoryTxnService'

export const configServices = {
    install: (app) => {

        const moduleServ = moduleService();
        const productServ = productService();
        const warehouseServ = warehouseService();
        const invTxnServ = inventoryTxnService();

        app.provide('services', {
            moduleServ,
            productServ,
            warehouseServ,
            invTxnServ
        });
    },
};
