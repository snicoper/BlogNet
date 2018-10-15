Vue.component('grid-column', {
  props: ['field', 'fieldName', 'tooltipHelper'],

  template: `
  <th @click="$parent.onOrderBy(field)" class="cursor-pointer">
      <span v-if="tooltipHelper" data-toggle="tooltip" :title="tooltipHelper">
          <i class="far fa-question-circle"></i>
      </span>
      {{ fieldName }} <span v-html="$parent.getCaretOrder(field)"></span>
  </th>`
})
