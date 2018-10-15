# crontab

Como usuario `crontab -e`

```bash
MAILTO=snicoper@snicoper.com

2 1 * * * /var/webapps/BlogNet/cron/postgres_db_backup.sh
```

Como root `sudo crontab -e`

```bash
MAILTO=snicoper@snicoper.com

30 2 * * 1 /usr/bin/certbot renew >> /var/log/le-renew.log
35 2 * * 1 /usr/bin/systemctl reload nginx
```
