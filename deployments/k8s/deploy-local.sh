# nginx-ingress
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.5/deploy/static/provider/baremetal/deploy.yaml
kubectl apply -f nginx-ingress/custom-service.yaml

# configmap
kubectl apply -f env-config.yaml

# movie-search-api
kubectl apply -f movie-search-api.yaml
kubectl apply -f movie-search-ingress.yaml