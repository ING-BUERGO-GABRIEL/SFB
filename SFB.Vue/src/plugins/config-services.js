import { moduleService } from '@/services/cms/moduleService'
import { productService } from '@/services/iaw/productService'

export const configServices = {
    install: (app) => {

        const moduleServ = moduleService();
        const productServ = productService();

        app.provide('services', {
            moduleServ,
            productServ
        });
    },
};
