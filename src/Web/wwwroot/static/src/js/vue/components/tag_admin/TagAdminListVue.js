/* globals gridMixin */

function TagAdminListVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#tag-admin-list-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/tag',

      // Filtros de búsqueda
      name: ''
    },

    watch: {
      name(val) {
        this.name = val
        this._onFilterChange()
      }
    },

    methods: {
      onFilterReset() {
        this.name = ''
        this._onFilterChange()
      },

      getLinkForEdit(id) {
        return `/admin/tag/edit/${id}/`
      },

      getLinkForDelete(id) {
        return `/admin/tag/delete/${id}/`
      },

      _getObjectList() {
        const url = `${this.url}/paginate/`
        this.loading = true

        axios({
          method: 'get',
          url: url,
          params: {
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            sortOrder: this.sortOrder,
            orderBy: this.orderBy,

            // Filtros de búsqueda
            name: this.name
          }
        })
          .then((response) => {
            const data = response.data
            this.objectList = data.objectList || []
            this.totalItems = data.totalItems
            this.totalPages = data.totalPages
            this.loading = false
          })
      }
    }
  })
}
