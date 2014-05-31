####Response variable FOR performance
perform<-c(69,74,117,NA,42,86,67,118,13,106,98,140,66,58,110,140)


#imputation (for estimation of missing value)

random.imp <- function (a){
missing <- is.na(a)
n.missing <- sum(missing)
a.obs <- a[!missing]
imputed <- a
imputed[missing] <- sample (a.obs, n.missing, replace=TRUE)
return (imputed)
}
a<-random.imp(perform)
perform <- random.imp (a)

#ANOVA1 (latin square)
experience<-rep(1:4,4)
order<-rep(1:4,each=4)
# Define A=condition1, B=condition2, C=condition3, D=condition4
condition<-c(4,1,2,3, 1,2,3,4, 3,4,1,2, 2,3,4,1)
experiencefactor<-factor(experience)
orderfactor<-factor(order)
conditionfactor<-factor(condition)
performfit<-aov(perform~conditionfactor+orderfactor+experiencefactor,
    contrasts=list(conditionfactor=contr.sum,orderfactor=contr.sum,experiencefactor=contr.sum))
anova(performfit)
B<-anova(performfit)

data.frame(cbind (perform,order,experience,condition))

TukeyHSD(performfit, conf.level = 0.95)

####estimations
coeff<-coef(performfit)
tau<-coeff[2:4]
taui<-c(tau,-sum(tau))
mui<-coeff[1]+taui
mui
CI<-mui-qt(0.975, 16-4)*sqrt(B[["Mean Sq"]][4])/4
CI2<-mui+qt(0.975, 16-4)*sqrt(B[["Mean Sq"]][4])/4
####################################################
####################################################
# Model Adequacy Checking

#######################################################################
r<-rstandard(performfit)
fitted<-fitted.values(performfit)

# Normal Q-Q plot
qqnorm(r,ylab="Standardized residuals",xlab="Theoretical quantiles",
   main="Normal Q-Q Plot of Player's Performances")
qqline(r)

# Plot of residuals vs. fitted values
plot(fitted,r,ylab="Standardized residuals", xlab="Fitted Values",
  main="Plot of Residuals vs. Fitted Values")
abline(h=0)

#######################################################################
#######################################################################
#######################################################################
#######################################################################
#effect of experience and conditions on exitemnet.
########################################################

gsr<-c(-17.2518,6.785,-10.7204,NA,-1.8195,58.3814,24.5918,104.7899,
10.2778,27.8437,42.3241,27.3695,15.0953,29.143,17.9926,29.7538)

#imputation (for estimation of missing value)

random.imp <- function (a){
missing <- is.na(a)
n.missing <- sum(missing)
a.obs <- a[!missing]
imputed <- a
imputed[missing] <- sample (a.obs, n.missing, replace=TRUE)
return (imputed)
}
a<-random.imp(gsr)
gsr<- random.imp (a)


#ANOVA2
experience<-rep(1:4,4)
condition<-rep(1:4,each=4)

experiencefactor<-factor(experience)
conditionfactor<-factor(condition)
gsrfit<-aov(gsr~conditionfactor+experiencefactor,
    contrasts=list(conditionfactor=contr.sum,experiencefactor=contr.sum))
anova(gsrfit)
A<-anova(gsrfit)


data.frame(cbind (gsr,experience,condition))
###############multiple comparison


TukeyHSD(gsrfit, conf.level = 0.95)

####mean estimation
coeff<-coef(gsrfit)
tau<-coeff[2:4]
taui<-c(tau,-sum(tau))
mui<-coeff[1]+taui
mui
# confidence intervals for means
CI<-mui-qt(0.95, 16-4)*sqrt(A[["Mean Sq"]][3]/4)
CI2<-mui+qt(0.95, 16-4)*sqrt(A[["Mean Sq"]][3]/4)

############model adequacy checking 
r<-rstandard(gsrfit)
fitted<-fitted.values(gsrfit)


# Normal Q-Q plot
qqnorm(r,ylab="Standardized residuals",xlab="Theoretical quantiles",
   main="Normal Q-Q Plot for the Excitement")
qqline(r)
# Plot of residuals vs. fitted values
plot(fitted,r,ylab="Standardized residuals", xlab="Fitted Values",
  main="Plot of Residuals vs. Fitted Values")
abline(h=0)
