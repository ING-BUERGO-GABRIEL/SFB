import { moduleService } from '@/services/cms/moduleService'
import { productService } from '@/services/iaw/productService'
import { warehouseService } from '@/services/iaw/warehouseService'

export const configServices = {
    install: (app) => {

        const moduleServ = moduleService();
        const productServ = productService();
        const warehouseServ = warehouseService();

        app.provide('services', {
            moduleServ,
            productServ,
            warehouseServ
        });
    },
};
