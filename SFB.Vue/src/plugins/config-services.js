import { moduleService } from '@/services/cms/moduleService'
//IAW
import { productService } from '@/services/iaw/productService'
import { warehouseService } from '@/services/iaw/warehouseService'
import { presentationService } from '@/services/iaw/presentationService'
import { inventoryTxnService } from '@/services/iaw/inventoryTxnService'
//PCM
import { supplierService } from '@/services/pcm/supplierService'
import { purchaseService } from '@/services/pcm/purchaseService'
//SOM
import { salesTxnService } from '@/services/som/salesTxn'
import { customerService } from '@/services/som/customer'
//TMR
import { cashBoxService } from '@/services/tmr/cashBox'
//AMS
import { amsPersonService } from '@/services/ams/amsPersonService'
//TAM
import { tamTeacherTaskService } from '@/services/tam/tamTeacherTaskService'
import { tamExamFormService } from '@/services/tam/tamExamFormService'
//UI
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
        const customerServ = customerService();
        const cashBoxServ = cashBoxService();
        const uiStore = useUIStore()

        const amsPersonServ = amsPersonService()

        const tamExamFormServ = tamExamFormService()
        const tamTeacherTaskServ = tamTeacherTaskService()

        app.provide('services', {
            moduleServ,
            productServ,
            warehouseServ,
            presentationServ,
            invTxnServ,
            supplierServ,
            purchaseServ,
            salesServ,
            customerServ,
            cashBoxServ,
            uiStore,
            amsPersonServ,
            tamTeacherTaskServ,
            tamExamFormServ
        });
    },
};
