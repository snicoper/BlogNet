/* globals gridMixin */

function LogEmailIndexVue() {
  Vue.component('paginate', VuejsPaginate)

  new Vue({
    el: '#log-email-index-vue',

    mixins: [gridMixin],

    data: {
      url: '/api/logemail',

      // Filtros de búsqueda
      from: '',
      to: '',
      subject: '',
      message: '',
      sendAt: ''
    },

    mounted() {
      $('#datetimepickerSendAt').datepicker(this.datePickerOptions)
        .on('changeDate', function () {
          this.sendAt = $('#datetimepickerSendAt').val()
        }.bind(this))
    },

    watch: {
      from(val) {
        this.from = val
        this._onFilterChange()
      },
      to(val) {
        this.to = val
        this._onFilterChange()
      },
      subject(val) {
        this.subject = val
        this._onFilterChange()
      },
      message(val) {
        this.message = val
        this._onFilterChange()
      },
      sendAt(val) {
        this.active = val
        this._onFilterChange()
      },
    },

    methods: {
      onDelete(id) {
        const url = `${this.url}/${id}`
        this._onDelete(url, id)
      },

      onFilterReset() {
        this.userName = ''
        this.email = ''
        this.sendAt = ''
        this.lastLogin = ''
        this.active = ''
        this._onFilterChange()
      },

      getLinkToLogEmailDetails(emailId) {
        return `/log-email/details/${emailId}/`
      },

      _getObjectList() {
        const url = `${this.url}/paginate/`
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
            from: this.from,
            to: this.to,
            subject: this.subject,
            message: this.message,
            sendAt: this.sendAt
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
