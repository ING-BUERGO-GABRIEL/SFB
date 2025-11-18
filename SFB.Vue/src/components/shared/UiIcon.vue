<template>
  <component
    :is="iconComponent"
    :style="iconStyle"
  />
</template>

<script setup>
import { computed } from 'vue'
import * as AntIcons from '@ant-design/icons-vue'

const props = defineProps({
  name: {
    type: String,
    default: 'QuestionCircleOutlined'
  },
  size: {
    type: [String, Number],
    default: 24
  },
  color: {
    type: String,
    default: 'currentColor'
  }
})

const iconComponent = computed(() => {
  return AntIcons[props.name] || AntIcons['QuestionCircleOutlined']
})

const normalizedSize = computed(() => {
  if (typeof props.size === 'number') return `${props.size}px`
  if (/^\d+$/.test(props.size)) return `${props.size}px`
  return props.size
})

const iconStyle = computed(() => ({
  fontSize: normalizedSize.value,
  color: props.color
}))
</script>
