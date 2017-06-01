close all
[FileName,PathName] = uigetfile('*.txt','Select a multiple of 10 trials .txt files','MultiSelect','on');

nFile = size(FileName,2);
nrows = 1000;
ncols = 20;

datadim = nFile/10;
datamat = zeros(nrows,ncols,datadim);
dim = 1;
count = 1;
maintitle = {}; 
maintitle{1}=FileName{1}(1:end-11);
if iscell(FileName)
    for i=1:nFile
    
    data=dlmread(strcat(PathName,FileName{i}),'\t');
    datamat(:,count*2-1:count*2,dim)=data(:,2:3);
    
        if i == nFile
            break;
        elseif count == 10
        
            maintitle{end+1} = FileName{i+1}(1:end-11);
            count = 1;
            dim = dim +1;
        else
            count=count+1;
            
        end
    end
end

timevalues = datamat(:,1:2:end,:);
boolvalues = datamat(:,2:2:end,:);
mintime = min(min(min(timevalues)));
maxtime = max(max(max(timevalues)));
maxavg = max(max(max(mean(timevalues))))+max(max(max(std(timevalues))));
minavg = min(min(min(mean(timevalues))))-max(max(max(std(timevalues))));
colors = {[0 1 1],[1 0 0],[1 1 0],[.4 1 1],[.5 .5 0],[.9 .6 .3],[.2 .3 .6],[.8 .4 .1],[1 0 .6],[0 .3 .3]};
for k = 1:datadim
time_test = timevalues(:,:,k);
bool_test = boolvalues(:,:,k);
avgvalues = mean(time_test);
stdev = std(time_test);

successrate = (sum(bool_test)/nrows)*100;
if successrate < 100
    display('Failed bool test')
    break;
end
nbins = round(nrows*nFile/datadim/4);

maxval = max(max(time_test));
minval = min(min(time_test));
avgval = mean(avgvalues);
medval = median(median(time_test));
range = maxval - minval;
h1=figure;


[N, edges] = histcounts(time_test);
maxy = max(N);
bar(edges(1:end-1),N,'EdgeColor','None')
%histogram(time_test,'EdgeColor','None')
LW=2;
axis([mintime maxtime 0 maxy]) 
ax=gca;
hold on;
line([maxval maxval],[ylim],'Color','r','LineStyle','--','LineWidth',LW)
line([minval minval],[ylim],'Color','m','LineStyle','--','LineWidth',LW)
line([avgval avgval],[ylim],'Color','g','LineStyle','--','LineWidth',LW)
%line([medval medval],[ylim],'Color','g','LineStyle','--')

%y=ylim;
strstats = sprintf('Min = %.2f ms, Mean = %.2f ms, Max = %.2f ms',minval,avgval,maxval);
% strmin = ['Min = ',num2str(minval)];
% text(minval,y(2)*3/4,strmin)%'HorizontalAlignment','right');
% 
% strmax = ['Max = ',num2str(maxval)];
% text(maxval-.1*range,y(2)*5.3/8,strmax,'HorizontalAlignment','right');
% 
% strmean = ['Mean = ',num2str(avgval)];
% text(avgval-.25*range,y(2)*5/8,strmean)%'HorizontalAlignment','right');
% % 
% % strmed = ['Median = ',num2str(medval)];
% % text(medval-.25*range,y(2)*3/8,strmed)%'HorizontalAlignment','right');
legend(ax,'histogram','max value','min value','avg value','Location','Best')


xlabel('Time delay (ms)')
ylabel('Number of instances')
str = {sprintf('Time delays for %s', maintitle{k}),strstats};
title(str,'Interpreter','None') 

file_hist = strcat(PathName,maintitle{k},'_hist.jpg');
file_hist2 = strcat(PathName,maintitle{k},'_histoverlay.jpg');
saveas(h1,file_hist)



h2 = figure;
bar(avgvalues);
axis([0 inf 0 maxavg])
hold on
errorbar(avgvalues,stdev,'.')
str2 = sprintf('Average time delays per trial for %s', maintitle{k});
title(str2, 'Interpreter','None');
xlabel('Trial number')
ylabel('Average time delay (ms)')


file_avg = strcat(PathName,maintitle{k},'_avgvals.jpg');
saveas(h2,file_avg)

h3 = figure;
bar(successrate);
xlabel('Trial number')
ylabel('Success rate (%)')
str3 = sprintf('Success rate per trial for %s', maintitle{k});
title(str3,'Interpreter','None');
axis([0 inf 0 120])

file_success = strcat(PathName,maintitle{k},'_successrate.jpg');
saveas(h3,file_success)

totarray = reshape(time_test,nrows*nFile/datadim,1);
h4 = figure;
plot(totarray,'o')
axis([0 10000 mintime maxtime])
xlabel('Data point')
ylabel('Time delay (ms)')
str4 = sprintf('Total data points for %s', maintitle{k});
title(str4,'Interpreter','None');

file_alldata = strcat(PathName,maintitle{k},'_alldata.jpg');
saveas(h4,file_alldata)

trialstats = zeros(ncols/2,5);
for j=1:ncols/2
    trial = time_test(:,j);
    mintrial = min(trial);
    maxtrial = max(trial);
    avgtrial = avgvalues(j);
    medtrial = median(trial);
    stdtrial = stdev(j);
    ranget = maxtrial - mintrial;
    trialstats(j,:)=[mintrial maxtrial avgtrial medtrial stdtrial];
    h5 = figure;
 [N, edges] = histcounts(trial);
 maxy = max(N);
 bar(ax,edges(1:end-1),N,'EdgeColor','None','FaceColor',colors{j})
histogram(trial,'EdgeColor','None') 
axis([mintime maxtime 0 maxy]) 
hold on
line([maxtrial maxtrial],[ylim],'Color','r','LineStyle','--','LineWidth',LW)
line([mintrial mintrial],[ylim],'Color','m','LineStyle','--','LineWidth',LW)
line([avgtrial avgtrial],[ylim],'Color','g','LineStyle','--','LineWidth',LW)
% line([medtrial medtrial],[ylim],'Color','g','LineStyle','--')
strstats = sprintf('Min = %.2f ms, Mean = %.2f ms, Max = %.2f ms',mintrial,avgtrial,maxtrial);

% y = ylim;
% 
% 
% strmint = ['Minimum = ',num2str(mintrial)];
% text(mintrial,y(2)*3/10,strmint)%'HorizontalAlignment','right');
% 
% strmaxt = ['Maximum = ',num2str(maxtrial)];
% text(maxtrial-.1*ranget,y(2)*6/10,strmaxt,'HorizontalAlignment','right');
% 
% strmeant = ['Mean = ',num2str(avgtrial)];
% text(avgtrial-.25*ranget,y(2)*5/10,strmeant)%'HorizontalAlignment','right');
% 
% strmedt = ['Median = ',num2str(medtrial)];
% text(medtrial-.25*ranget,y(2)*4/10,strmedt);%'HorizontalAlignment','right');

legend('histogram','max value','min value','avg value','Location','Best');
 xlabel('Time delay (ms)')
ylabel('Number of instances')
str5 = {sprintf('Time delays for %s, trial %d', maintitle{k}, j),strstats};
title(str5,'Interpreter','None')
   
file_hist_trial = strcat(PathName,maintitle{k},'trial_',num2str(j),'_hist.jpg');
saveas(h5,file_hist_trial)
     
 
    
end
legend(ax,'histogram','max value','min value','avg value','Trial 1','Trial 2','Trial 3','Trial 4','Trial 5','Trial 6','Trial 7','Trial 8','Trial 9','Trial 10','Location','Best')
saveas(h1,file_hist2)

h6 = figure;
boxplot(time_test)
axis([0 inf mintime maxtime])
xlabel('Trial number')
ylabel('Time delay (ms)')
str6 = sprintf('Boxplot for %s',maintitle{k});
title(str6,'Interpreter','None')
file_boxplot = strcat(PathName,maintitle{k},'_boxplot.jpg');
saveas(h6,file_boxplot)

fid = fopen(strcat(PathName,maintitle{k},'_stats.csv','w'));

for i = 1:1000:9001
    newtime(i:i+999)=time_test(:,(i-1)/1000+1);
end
stdall = std(newtime);
Stats = {'Minimum','Maximum','Average','Median','Standard Deviation';minval,maxval,avgval,medval,stdall};
fid = fopen(fullfile(PathName,strcat(maintitle{k},'_stats.csv')),'w');

fprintf(fid,'%s,%s,%s,%s,%s\n',Stats{1,:});
fprintf(fid,'%f,%f,%f,%f,%f\n',Stats{2,:});
fprintf(fid,'%s\n','Trials');
dlmwrite(fullfile(PathName,strcat(maintitle{k},'_stats.csv')),trialstats,'-append');
for j=1:ncols/2
    fprintf(fid,'%f,%f,%f,%f,%f\n',trialstats(j,:));
end
fclose(fid);

end