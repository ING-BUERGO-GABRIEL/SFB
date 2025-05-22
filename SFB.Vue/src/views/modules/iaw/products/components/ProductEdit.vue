<template>
  <div class="product-edit-wrapper">
    <div class="full-cover">
      <h1>Hola</h1>
    </div>
  </div>
</template>

<script setup>
// sin cambios aquí
</script>

<style scoped>
.product-edit-wrapper {
  position: relative;
  width: 100%;
  height: 100%; /* heredado por fill-height */
}

.full-cover {
  position: absolute;
  inset: 0;                /* equivale a top:0; right:0; bottom:0; left:0 */
  background-color: blue;
  display: flex;           /* opcional: centra el contenido */
  align-items: center;
  justify-content: center;
  color: white;            /* para que se lea “Hola” */
}
</style>
