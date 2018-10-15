/* global gridMixin */

function ArticleAdminListVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#article-admin-list-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/article',

      // Filtros de búsqueda
      title: '',
      body: '',
      active: 'None',
      createAt: '',
      updateAt: '',
      owner: '',
      defaultTag: 0
    },

    mounted() {
      $('#datetimepickerCreateAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.createAt = $('#datetimepickerCreateAt').val()
        }.bind(this))

      $('#datetimepickerUpdateAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.updateAt = $('#datetimepickerUpdateAt').val()
        }.bind(this))
    },

    watch: {
      title(val) {
        this.title = val
        this._onFilterChange()
      },
      body(val) {
        this.body = val
        this._onFilterChange()
      },
      active(val) {
        this.active = val
        this._onFilterChange()
      },
      createAt(val) {
        this.createAt = val
        this._onFilterChange()
      },
      updateAt(val) {
        this.updateAt = val
        this._onFilterChange()
      },
      owner(val) {
        this.owner = val
        this._onFilterChange()
      },
      defaultTag(val) {
        this.defaultTag = val
        this._onFilterChange()
      }
    },

    methods: {
      onFilterReset() {
        this.title = ''
        this.body = ''
        this.active = 'None'
        this.createAt = ''
        this.updateAt = ''
        this.owner = ''
        this.defaultTag = 0
        this._onFilterChange()
      },

      getLinkEdit(id) {
        return `/admin/article/edit/${id}/`
      },

      getLinkDetails(slug) {
        return `/article/details/${slug}/`
      },

      onDelete(id) {
        const url = `${this.url}/${id}`
        this._onDelete(url, id)
      },

      _getObjectList() {
        const url = `${this.url}/paginate`
        this.loading = true

        axios({
          method: 'get',
          url: url,
          params: {
            // Paginación
            pageNumber: this.pageNumber,
            pageSize: this.pageSize,
            sortOrder: this.sortOrder,
            orderBy: this.orderBy,

            // Filtros de búsqueda
            title: this.title,
            body: this.body,
            active: this.active,
            createAt: this.createAt,
            updateAt: this.updateAt,
            owner: this.owner,
            defaultTag: this.defaultTag
          },
        })
          .then((response) => {
            const data = response.data
            this.objectList = data.objectList
            this.totalItems = data.totalItems
            this.totalPages = data.totalPages
            this.loading = false
          })
      }
    }
  })
}
