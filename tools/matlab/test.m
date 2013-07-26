%----------------------------------------------
%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_1\20130605_2004_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_2\20130607_1729_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_3\20130611_1458_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_4\20130612_1322_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1515_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1455_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_5\20130612_1455_metrics.csv', 16);

%[arousal_h, arousal_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1703_arousal.csv', 3);
%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1619_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\pilot_6\20130612_1619_metrics.csv', 16);

%[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\experiment\1\20130711_0921_gsr.csv', 3);
%[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\experiment\1\20130711_0921_metrics.csv', 16);

[gsr_h, gsr_v]         = readCSV('D:\faham\emophiz\emophiz\logs\experiment\10\20130711_0921_gsr.csv', 3);
[metrics_h, metrics_v] = readCSV('D:\faham\emophiz\emophiz\logs\experiment\10\20130711_0921_metrics.csv', 16);

%----------------------------------------------
%1 time_millisecond
%2 arousal
%3 player_speed
%4 zombie_speed
%5 fog_start_dist
%6 fog_end_dist
%7 current_round
%8 zombie_threshold
%9 zombie_increase_power
%10 max_zombie_alive
%11 number_of_alive_zombies
%12 number_of_killed_zombies
%13 grenade_regen_delay
%14 medic_regen_delay
%15 calibrating
%16 adaptation_condition
%----------------------------------------------
%finding conditions
%m_conditions = {[], [], [], []}
%
%m_cur_cond = 0
%m_start = -1
%m_end = -1
%for i=1:length(metrics_v{16}) - 1
%	if metrics_v{16}(i) ~= 0 && m_start == -1
%		m_start = i
%		m_cur_cond = metrics_v{16}(i)
%	elseif metrics_v{16}(i) ~= 0 && (metrics_v{16}(i + 1) == 0 || i + 1 == length(metrics_v{16}))
%		if m_end == -1
%			m_end = i
%			m_conditions{m_cur_cond} = [m_start, m_end]
%		end
%	elseif metrics_v{16}(i) == 0 && (metrics_v{16}(i + 1) ~= 0)
%		if m_end ~= -1 && m_start ~= -1 && m_cur_cond ~= 0
%			m_end = -1
%			m_start = -1
%			m_cur_cond = 0
%		end
%	end
%end
%%----------------------------------------------
%for i=1:length(m_conditions)
%	start_time = metrics_v{1}(m_conditions{i}(1))
%	end_time = metrics_v{1}(m_conditions{i}(2))
%	start_gsr_index = -1
%	end_gsr_index = -1
%	for j=1:length(gsr_v{1})
%		if gsr_v{1}(j) >= start_time && start_gsr_index == -1
%			start_gsr_index = j
%		elseif gsr_v{1}(j) >= end_time && end_gsr_index == -1
%			end_gsr_index = j
%		end
%	end
%	%for each condition the array is as the following:
%	%1: condition start time index
%	%2: condition end time index
%	%3: gsr start value
%	%4: gsr end value
%	%5: gsr mean
%	%6: gsr variance
%	%7: gsr slope
%	m_conditions{i}(3) = gsr_v{2}(start_gsr_index)
%	m_conditions{i}(4) = gsr_v{2}(end_gsr_index)
%	m_conditions{i}(5) = mean(gsr_v{2}(start_gsr_index:end_gsr_index))
%	m_conditions{i}(6) = var(gsr_v{2}(start_gsr_index:end_gsr_index), 1)
%	m_conditions{i}(7) = diff(mean(gsr_v{2}(start_gsr_index:start_gsr_index + (end_gsr_index - start_gsr_index) / 2)), mean(gsr_v{2}(start_gsr_index + (end_gsr_index - start_gsr_index) / 2:end)))
%end
%----------------------------------------------

plot(gsr_v{1}, (gsr_v{2} * 1000) + 800 , 'b-'); hold on;
%plot(gsr_v{1}, smooth(gsr_v{1}, (gsr_v{2} * 1000) + 800, 0.1, 'loess'), 'r-'); hold on; 
%plot(gsr_v{1}, gsr_v{2}, 'b.', gsr_v{1}, smooth(gsr_v{1}, (gsr_v{2} * 1000) + 800, 0.1, 'loess'),'r-')
plot(metrics_v{1}, metrics_v{15} * 100 + 25, 'k-'); hold on; % calibration
plot(metrics_v{1}, metrics_v{16} * 20 + 50, 'c-'); hold on; % adaptation
%plot(bvp_v{1}, bvp_v{3}, 'c-'); %hold on;
%plot(hr_v{1}, hr_v{3}, 'g-'); hold on; 
%plot(arousal_v{1}, arousal_v{3}, 'r-'); hold on; 
%plot(metrics_v{1}, metrics_v{2} * 100, 'b-'); hold on; % arousal
plot(metrics_v{1}, (metrics_v{3}) * 100 / 2.3, 'y-'); %hold on; % player speed
plot(metrics_v{1}, (metrics_v{4} - 1 ) * 100 / 3, 'g-'); %hold on; % zombie speed
plot(metrics_v{1}, (metrics_v{5} - 70) * 100 / 380, 'm-'); %hold on; % fog distant
legend('GSR Data', 'Calibration', 'Condition', 'Player Speed', 'Zombie Speed', 'Fog Distant', 'Location','NW')
%----------------------------------------------
xlabel('time');
ylabel('signal');
%grid on;
%----------------------------------------------
