# Etapa 1: Construção da aplicação Angular
FROM node:20.11.1-alpine AS build

WORKDIR /app

# Copia os arquivos de dependências
COPY package.json package-lock.json ./

# Instala as dependências
RUN npm ci

# Copia o restante dos arquivos e faz o build da aplicação
COPY . .
RUN npm install -g @angular/cli
RUN npm run production

# Etapa 2: Servindo os arquivos estáticos com Nginx
FROM nginx:1.25-alpine

# Remove a configuração padrão do Nginx
RUN rm -rf /etc/nginx/conf.d/default.conf

# Copia a configuração customizada do Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*

# Copia os arquivos da aplicação Angular para o diretório público do Nginx
COPY --from=build /app/dist/employees-fe/browser /usr/share/nginx/html

# Expõe a porta 80 para acesso externo
EXPOSE 80

# Inicia o Nginx
CMD ["nginx", "-g", "daemon off;"]
