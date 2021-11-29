# configmap
kubectl delete cm env-config

# movie-search-api
kubectl delete deploy movie.api-dpl 
kubectl delete ing movie.api-ingress
kubectl delete svc movie.api-svc-v1
